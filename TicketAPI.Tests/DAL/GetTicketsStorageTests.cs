using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.GetTickets;
using TicketAPI.Domain.Enums;
using TicketAPI.Tests.DAL.Infrastructure;

namespace TicketAPI.Tests.DAL;

[Collection("SqlServer")]
public class GetTicketsStorageTests : IAsyncLifetime
{
    private readonly SqlServerContainerFixture _fixture;
    private TicketDbContext _context = null!;
    private IDbContextTransaction _transaction = null!;

    public GetTicketsStorageTests(SqlServerContainerFixture fixture) => _fixture = fixture;

    public async Task InitializeAsync()
    {
        _context = new TicketDbContext(
            new DbContextOptionsBuilder<TicketDbContext>()
                .UseSqlServer(_fixture.ConnectionString)
                .Options);
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task DisposeAsync()
    {
        await _transaction.RollbackAsync();
        await _context.DisposeAsync();
    }

    private void SeedMany(IEnumerable<Ticket> tickets)
    {
        _context.Tickets.AddRange(tickets);
        _context.SaveChanges();
    }

    private static Ticket MakeTicket(
        Guid? eventId = null,
        Guid? orderItemId = null,
        Guid? seatId = null,
        ETicketStatus status = ETicketStatus.Unused)
        => new()
        {
            Id = Guid.NewGuid(),
            OrderItemId = orderItemId ?? Guid.NewGuid(),
            EventId = eventId ?? Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            SeatId = seatId,
            QRCode = Guid.NewGuid().ToString(),
            Status = status,
            CreatedAt = DateTimeOffset.UtcNow
        };

    [Fact]
    public async Task GetAsync_ReturnsAll_WhenNoFilters()
    {
        SeedMany([MakeTicket(), MakeTicket(), MakeTicket()]);
        var storage = new GetTicketsStorage(_context);

        var result = await storage.GetAsync(1, 10, null, null, null, null, CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.Equal(3, result.Data!.Count);
    }

    [Fact]
    public async Task GetAsync_FiltersByEventId()
    {
        var eventId = Guid.NewGuid();
        SeedMany([MakeTicket(eventId: eventId), MakeTicket(eventId: eventId), MakeTicket()]);
        var storage = new GetTicketsStorage(_context);

        var result = await storage.GetAsync(1, 10, eventId, null, null, null, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Data!, t => Assert.Equal(eventId, t.EventId));
    }

    [Fact]
    public async Task GetAsync_FiltersByOrderItemId()
    {
        var orderItemId = Guid.NewGuid();
        SeedMany([MakeTicket(orderItemId: orderItemId), MakeTicket()]);
        var storage = new GetTicketsStorage(_context);

        var result = await storage.GetAsync(1, 10, null, orderItemId, null, null, CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Equal(orderItemId, result.Data![0].OrderItemId);
    }

    [Fact]
    public async Task GetAsync_FiltersBySeatId()
    {
        var seatId = Guid.NewGuid();
        SeedMany([MakeTicket(seatId: seatId), MakeTicket()]);
        var storage = new GetTicketsStorage(_context);

        var result = await storage.GetAsync(1, 10, null, null, seatId, null, CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Equal(seatId, result.Data![0].SeatId);
    }

    [Fact]
    public async Task GetAsync_FiltersByStatus()
    {
        SeedMany([
            MakeTicket(status: ETicketStatus.Unused),
            MakeTicket(status: ETicketStatus.Used),
            MakeTicket(status: ETicketStatus.Unused)
        ]);
        var storage = new GetTicketsStorage(_context);

        var result = await storage.GetAsync(1, 10, null, null, null, ETicketStatus.Unused, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Data!, t => Assert.Equal(ETicketStatus.Unused, t.Status));
    }

    [Fact]
    public async Task GetAsync_PaginatesCorrectly()
    {
        SeedMany(Enumerable.Range(0, 6).Select(_ => MakeTicket()));
        var storage = new GetTicketsStorage(_context);

        var result = await storage.GetAsync(2, 3, null, null, null, null, CancellationToken.None);

        Assert.Equal(6, result.Count);
        Assert.Equal(3, result.Data!.Count);
    }

    [Fact]
    public async Task GetAsync_ReturnsEmpty_WhenNoMatch()
    {
        SeedMany([MakeTicket(status: ETicketStatus.Unused)]);
        var storage = new GetTicketsStorage(_context);

        var result = await storage.GetAsync(1, 10, null, null, null, ETicketStatus.Cancelled, CancellationToken.None);

        Assert.Equal(0, result.Count);
        Assert.Empty(result.Data!);
    }
}

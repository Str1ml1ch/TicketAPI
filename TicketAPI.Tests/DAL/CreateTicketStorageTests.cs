using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TicketAPI.DAL;
using TicketAPI.DAL.Storage.CreateTicket;
using TicketAPI.Domain.Enums;
using TicketAPI.Tests.DAL.Infrastructure;

namespace TicketAPI.Tests.DAL;

[Collection("SqlServer")]
public class CreateTicketStorageTests : IAsyncLifetime
{
    private readonly SqlServerContainerFixture _fixture;
    private TicketDbContext _context = null!;
    private IDbContextTransaction _transaction = null!;

    public CreateTicketStorageTests(SqlServerContainerFixture fixture) => _fixture = fixture;

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

    [Fact]
    public async Task CreateAsync_PersistsTicket_AndReturnsNewId()
    {
        var storage = new CreateTicketStorage(_context);
        var orderItemId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var sectionId = Guid.NewGuid();
        var seatId = Guid.NewGuid();

        var id = await storage.CreateAsync(orderItemId, eventId, sectionId, seatId, "QR-001", ETicketStatus.Unused, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        var ticket = _context.Tickets.Single(t => t.Id == id);
        Assert.Equal(orderItemId, ticket.OrderItemId);
        Assert.Equal(eventId, ticket.EventId);
        Assert.Equal(sectionId, ticket.SectionId);
        Assert.Equal(seatId, ticket.SeatId);
        Assert.Equal("QR-001", ticket.QRCode);
        Assert.Equal(ETicketStatus.Unused, ticket.Status);
    }

    [Fact]
    public async Task CreateAsync_AllowsNullSeatId()
    {
        var storage = new CreateTicketStorage(_context);

        var id = await storage.CreateAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), null, "QR-GA", ETicketStatus.Unused, CancellationToken.None);

        var ticket = _context.Tickets.Single(t => t.Id == id);
        Assert.Null(ticket.SeatId);
    }
}

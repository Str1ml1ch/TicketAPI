using Microsoft.EntityFrameworkCore;
using TicketAPI.Domain.Enums;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.GetTickets;

namespace TicketAPI.Tests.DAL;

public class GetTicketsStorageTests
{
    private static TicketDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TicketDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TicketDbContext(options);
    }

    private static void SeedMany(TicketDbContext ctx, IEnumerable<Ticket> tickets)
    {
        ctx.Tickets.AddRange(tickets);
        ctx.SaveChanges();
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
        using var ctx = CreateContext();
        SeedMany(ctx, [MakeTicket(), MakeTicket(), MakeTicket()]);
        var storage = new GetTicketsStorage(ctx);

        var result = await storage.GetAsync(1, 10, null, null, null, null, CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.Equal(3, result.Data!.Count);
    }

    [Fact]
    public async Task GetAsync_FiltersByEventId()
    {
        using var ctx = CreateContext();
        var eventId = Guid.NewGuid();
        SeedMany(ctx, [MakeTicket(eventId: eventId), MakeTicket(eventId: eventId), MakeTicket()]);
        var storage = new GetTicketsStorage(ctx);

        var result = await storage.GetAsync(1, 10, eventId, null, null, null, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Data!, t => Assert.Equal(eventId, t.EventId));
    }

    [Fact]
    public async Task GetAsync_FiltersByOrderItemId()
    {
        using var ctx = CreateContext();
        var orderItemId = Guid.NewGuid();
        SeedMany(ctx, [MakeTicket(orderItemId: orderItemId), MakeTicket()]);
        var storage = new GetTicketsStorage(ctx);

        var result = await storage.GetAsync(1, 10, null, orderItemId, null, null, CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Equal(orderItemId, result.Data![0].OrderItemId);
    }

    [Fact]
    public async Task GetAsync_FiltersBySeatId()
    {
        using var ctx = CreateContext();
        var seatId = Guid.NewGuid();
        SeedMany(ctx, [MakeTicket(seatId: seatId), MakeTicket()]);
        var storage = new GetTicketsStorage(ctx);

        var result = await storage.GetAsync(1, 10, null, null, seatId, null, CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Equal(seatId, result.Data![0].SeatId);
    }

    [Fact]
    public async Task GetAsync_FiltersByStatus()
    {
        using var ctx = CreateContext();
        SeedMany(ctx, [
            MakeTicket(status: ETicketStatus.Unused),
            MakeTicket(status: ETicketStatus.Used),
            MakeTicket(status: ETicketStatus.Unused)
        ]);
        var storage = new GetTicketsStorage(ctx);

        var result = await storage.GetAsync(1, 10, null, null, null, ETicketStatus.Unused, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Data!, t => Assert.Equal(ETicketStatus.Unused, t.Status));
    }

    [Fact]
    public async Task GetAsync_PaginatesCorrectly()
    {
        using var ctx = CreateContext();
        SeedMany(ctx, Enumerable.Range(0, 6).Select(_ => MakeTicket()));
        var storage = new GetTicketsStorage(ctx);

        var result = await storage.GetAsync(2, 3, null, null, null, null, CancellationToken.None);

        Assert.Equal(6, result.Count);
        Assert.Equal(3, result.Data!.Count);
    }

    [Fact]
    public async Task GetAsync_ReturnsEmpty_WhenNoMatch()
    {
        using var ctx = CreateContext();
        SeedMany(ctx, [MakeTicket(status: ETicketStatus.Unused)]);
        var storage = new GetTicketsStorage(ctx);

        var result = await storage.GetAsync(1, 10, null, null, null, ETicketStatus.Cancelled, CancellationToken.None);

        Assert.Equal(0, result.Count);
        Assert.Empty(result.Data!);
    }
}

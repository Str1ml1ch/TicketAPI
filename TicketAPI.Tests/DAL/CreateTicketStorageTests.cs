using Microsoft.EntityFrameworkCore;
using TicketAPI.Domain.Enums;
using TicketAPI.DAL;
using TicketAPI.DAL.Storage.CreateTicket;

namespace TicketAPI.Tests.DAL;

public class CreateTicketStorageTests
{
    private static TicketDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TicketDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TicketDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_PersistsTicket_AndReturnsNewId()
    {
        using var ctx = CreateContext();
        var storage = new CreateTicketStorage(ctx);
        var orderItemId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var sectionId = Guid.NewGuid();
        var seatId = Guid.NewGuid();

        var id = await storage.CreateAsync(orderItemId, eventId, sectionId, seatId, "QR-001", ETicketStatus.Unused, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        var ticket = ctx.Tickets.Single(t => t.Id == id);
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
        using var ctx = CreateContext();
        var storage = new CreateTicketStorage(ctx);

        var id = await storage.CreateAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), null, "QR-GA", ETicketStatus.Unused, CancellationToken.None);

        var ticket = ctx.Tickets.Single(t => t.Id == id);
        Assert.Null(ticket.SeatId);
    }
}

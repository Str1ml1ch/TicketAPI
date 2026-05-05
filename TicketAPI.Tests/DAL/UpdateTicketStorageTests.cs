using Microsoft.EntityFrameworkCore;
using TicketAPI.Domain.Enums;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.UpdateTicket;

namespace TicketAPI.Tests.DAL;

public class UpdateTicketStorageTests
{
    private static TicketDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TicketDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TicketDbContext(options);
    }

    private static Ticket Seed(TicketDbContext ctx)
    {
        var t = new Ticket
        {
            Id = Guid.NewGuid(),
            OrderItemId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            QRCode = "QR-UPDATE",
            Status = ETicketStatus.Unused,
            CreatedAt = DateTimeOffset.UtcNow
        };
        ctx.Tickets.Add(t);
        ctx.SaveChanges();
        return t;
    }

    [Fact]
    public async Task UpdateStatusAsync_ChangesStatus()
    {
        using var ctx = CreateContext();
        var ticket = Seed(ctx);
        var storage = new UpdateTicketStorage(ctx);
        var usedAt = DateTimeOffset.UtcNow;

        await storage.UpdateStatusAsync(ticket.Id, ETicketStatus.Used, usedAt, CancellationToken.None);

        var updated = ctx.Tickets.Find(ticket.Id)!;
        Assert.Equal(ETicketStatus.Used, updated.Status);
        Assert.Equal(usedAt, updated.UsedAt);
        Assert.NotNull(updated.UpdatedAt);
    }

    [Fact]
    public async Task UpdateStatusAsync_ToCancelled_SetsNullUsedAt()
    {
        using var ctx = CreateContext();
        var ticket = Seed(ctx);
        var storage = new UpdateTicketStorage(ctx);

        await storage.UpdateStatusAsync(ticket.Id, ETicketStatus.Cancelled, null, CancellationToken.None);

        var updated = ctx.Tickets.Find(ticket.Id)!;
        Assert.Equal(ETicketStatus.Cancelled, updated.Status);
        Assert.Null(updated.UsedAt);
    }
}

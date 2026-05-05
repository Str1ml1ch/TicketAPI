using Microsoft.EntityFrameworkCore;
using TicketAPI.Domain.Enums;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.GetTicketById;

namespace TicketAPI.Tests.DAL;

public class GetTicketByIdStorageTests
{
    private static TicketDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TicketDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TicketDbContext(options);
    }

    private static Ticket Seed(TicketDbContext ctx, string qrCode = "QR-1", ETicketStatus status = ETicketStatus.Unused)
    {
        var t = new Ticket
        {
            Id = Guid.NewGuid(),
            OrderItemId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            QRCode = qrCode,
            Status = status,
            CreatedAt = DateTimeOffset.UtcNow
        };
        ctx.Tickets.Add(t);
        ctx.SaveChanges();
        return t;
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx = CreateContext();
        var storage = new GetTicketByIdStorage(ctx);

        var result = await storage.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDetailModel_WhenFound()
    {
        using var ctx = CreateContext();
        var ticket = Seed(ctx, "QR-TEST");
        var storage = new GetTicketByIdStorage(ctx);

        var result = await storage.GetByIdAsync(ticket.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(ticket.Id, result.Id);
        Assert.Equal("QR-TEST", result.QRCode);
        Assert.Empty(result.Validations);
    }

    [Fact]
    public async Task GetByQrCodeAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx = CreateContext();
        var storage = new GetTicketByIdStorage(ctx);

        var result = await storage.GetByQrCodeAsync("NONEXISTENT", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByQrCodeAsync_ReturnsModel_WhenFound()
    {
        using var ctx = CreateContext();
        Seed(ctx, "QR-ABC");
        var storage = new GetTicketByIdStorage(ctx);

        var result = await storage.GetByQrCodeAsync("QR-ABC", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("QR-ABC", result.QRCode);
    }

    [Fact]
    public async Task IsExistsAsync_ReturnsFalse_WhenNotFound()
    {
        using var ctx = CreateContext();
        var storage = new GetTicketByIdStorage(ctx);

        var result = await storage.IsExistsAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task IsExistsAsync_ReturnsTrue_WhenFound()
    {
        using var ctx = CreateContext();
        var ticket = Seed(ctx);
        var storage = new GetTicketByIdStorage(ctx);

        var result = await storage.IsExistsAsync(ticket.Id, CancellationToken.None);

        Assert.True(result);
    }
}

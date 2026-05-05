using Microsoft.EntityFrameworkCore;
using TicketAPI.Domain.Enums;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.CreateScannerEvent;
using TicketAPI.DAL.Storage.GetScannerEvents;
using TicketAPI.DAL.Storage.RemoveScannerEvent;

namespace TicketAPI.Tests.DAL;

public class ScannerEventStorageTests
{
    private static TicketDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TicketDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TicketDbContext(options);
    }

    private static Scanner SeedScanner(TicketDbContext ctx, string serial = "SN-X")
    {
        var s = new Scanner { Id = Guid.NewGuid(), SerialNumber = serial, Status = EScannerStatus.Active, CreatedAt = DateTimeOffset.UtcNow };
        ctx.Scanners.Add(s);
        ctx.SaveChanges();
        return s;
    }

    private static ScannerEvent SeedEvent(TicketDbContext ctx, Guid scannerId, Guid? eventId = null)
    {
        var se = new ScannerEvent { Id = Guid.NewGuid(), ScannerId = scannerId, EventId = eventId ?? Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow };
        ctx.ScannerEvents.Add(se);
        ctx.SaveChanges();
        return se;
    }

    // --- CreateScannerEventStorage ---

    [Fact]
    public async Task CreateAsync_PersistsScannerEvent_AndReturnsNewId()
    {
        using var ctx = CreateContext();
        var scanner = SeedScanner(ctx);
        var storage = new CreateScannerEventStorage(ctx);
        var eventId = Guid.NewGuid();

        var id = await storage.CreateAsync(scanner.Id, eventId, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        var se = ctx.ScannerEvents.Single(e => e.Id == id);
        Assert.Equal(scanner.Id, se.ScannerId);
        Assert.Equal(eventId, se.EventId);
    }

    // --- GetScannerEventsStorage ---

    [Fact]
    public async Task GetAsync_FiltersByScannerId()
    {
        using var ctx = CreateContext();
        var scanner1 = SeedScanner(ctx, "SN-1");
        var scanner2 = SeedScanner(ctx, "SN-2");
        SeedEvent(ctx, scanner1.Id);
        SeedEvent(ctx, scanner1.Id);
        SeedEvent(ctx, scanner2.Id);
        var storage = new GetScannerEventsStorage(ctx);

        var result = await storage.GetAsync(1, 10, scanner1.Id, null, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Data!, e => Assert.Equal(scanner1.Id, e.ScannerId));
    }

    [Fact]
    public async Task GetAsync_FiltersByEventId()
    {
        using var ctx = CreateContext();
        var scanner = SeedScanner(ctx);
        var eventId = Guid.NewGuid();
        SeedEvent(ctx, scanner.Id, eventId);
        SeedEvent(ctx, scanner.Id);
        var storage = new GetScannerEventsStorage(ctx);

        var result = await storage.GetAsync(1, 10, null, eventId, CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Equal(eventId, result.Data![0].EventId);
    }

    [Fact]
    public async Task GetAsync_PaginatesCorrectly()
    {
        using var ctx = CreateContext();
        var scanner = SeedScanner(ctx);
        for (int i = 0; i < 5; i++) SeedEvent(ctx, scanner.Id);
        var storage = new GetScannerEventsStorage(ctx);

        var result = await storage.GetAsync(1, 3, scanner.Id, null, CancellationToken.None);

        Assert.Equal(5, result.Count);
        Assert.Equal(3, result.Data!.Count);
    }

    // --- RemoveScannerEventStorage ---

    [Fact]
    public async Task RemoveByIdAsync_RemovesSingleEvent()
    {
        using var ctx = CreateContext();
        var scanner = SeedScanner(ctx);
        var se = SeedEvent(ctx, scanner.Id);
        var storage = new RemoveScannerEventStorage(ctx);

        await storage.RemoveByIdAsync(se.Id, CancellationToken.None);

        Assert.False(ctx.ScannerEvents.Any(e => e.Id == se.Id));
    }

    [Fact]
    public async Task RemoveAllByScannerIdAsync_RemovesAllEventsForScanner()
    {
        using var ctx = CreateContext();
        var scanner1 = SeedScanner(ctx, "SN-A");
        var scanner2 = SeedScanner(ctx, "SN-B");
        SeedEvent(ctx, scanner1.Id);
        SeedEvent(ctx, scanner1.Id);
        SeedEvent(ctx, scanner2.Id);
        var storage = new RemoveScannerEventStorage(ctx);

        await storage.RemoveAllByScannerIdAsync(scanner1.Id, CancellationToken.None);

        Assert.Empty(ctx.ScannerEvents.Where(e => e.ScannerId == scanner1.Id));
        Assert.Single(ctx.ScannerEvents.Where(e => e.ScannerId == scanner2.Id));
    }
}

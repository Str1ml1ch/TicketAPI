using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.CreateScannerEvent;
using TicketAPI.DAL.Storage.GetScannerEvents;
using TicketAPI.DAL.Storage.RemoveScannerEvent;
using TicketAPI.Domain.Enums;
using TicketAPI.Tests.DAL.Infrastructure;

namespace TicketAPI.Tests.DAL;

[Collection("SqlServer")]
public class ScannerEventStorageTests : IAsyncLifetime
{
    private readonly SqlServerContainerFixture _fixture;
    private TicketDbContext _context = null!;
    private IDbContextTransaction _transaction = null!;

    public ScannerEventStorageTests(SqlServerContainerFixture fixture) => _fixture = fixture;

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

    private Scanner SeedScanner(string serial = "SN-X")
    {
        var s = new Scanner { Id = Guid.NewGuid(), SerialNumber = serial, Status = EScannerStatus.Active, CreatedAt = DateTimeOffset.UtcNow };
        _context.Scanners.Add(s);
        _context.SaveChanges();
        return s;
    }

    private ScannerEvent SeedEvent(Guid scannerId, Guid? eventId = null)
    {
        var se = new ScannerEvent { Id = Guid.NewGuid(), ScannerId = scannerId, EventId = eventId ?? Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow };
        _context.ScannerEvents.Add(se);
        _context.SaveChanges();
        return se;
    }

    // --- CreateScannerEventStorage ---

    [Fact]
    public async Task CreateAsync_PersistsScannerEvent_AndReturnsNewId()
    {
        var scanner = SeedScanner();
        var storage = new CreateScannerEventStorage(_context);
        var eventId = Guid.NewGuid();

        var id = await storage.CreateAsync(scanner.Id, eventId, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        var se = _context.ScannerEvents.Single(e => e.Id == id);
        Assert.Equal(scanner.Id, se.ScannerId);
        Assert.Equal(eventId, se.EventId);
    }

    // --- GetScannerEventsStorage ---

    [Fact]
    public async Task GetAsync_FiltersByScannerId()
    {
        var scanner1 = SeedScanner("SN-1");
        var scanner2 = SeedScanner("SN-2");
        SeedEvent(scanner1.Id);
        SeedEvent(scanner1.Id);
        SeedEvent(scanner2.Id);
        var storage = new GetScannerEventsStorage(_context);

        var result = await storage.GetAsync(1, 10, scanner1.Id, null, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Data!, e => Assert.Equal(scanner1.Id, e.ScannerId));
    }

    [Fact]
    public async Task GetAsync_FiltersByEventId()
    {
        var scanner = SeedScanner();
        var eventId = Guid.NewGuid();
        SeedEvent(scanner.Id, eventId);
        SeedEvent(scanner.Id);
        var storage = new GetScannerEventsStorage(_context);

        var result = await storage.GetAsync(1, 10, null, eventId, CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Equal(eventId, result.Data![0].EventId);
    }

    [Fact]
    public async Task GetAsync_PaginatesCorrectly()
    {
        var scanner = SeedScanner();
        for (int i = 0; i < 5; i++) SeedEvent(scanner.Id);
        var storage = new GetScannerEventsStorage(_context);

        var result = await storage.GetAsync(1, 3, scanner.Id, null, CancellationToken.None);

        Assert.Equal(5, result.Count);
        Assert.Equal(3, result.Data!.Count);
    }

    // --- RemoveScannerEventStorage ---

    [Fact]
    public async Task RemoveByIdAsync_RemovesSingleEvent()
    {
        var scanner = SeedScanner();
        var se = SeedEvent(scanner.Id);
        var storage = new RemoveScannerEventStorage(_context);

        await storage.RemoveByIdAsync(se.Id, CancellationToken.None);

        Assert.False(_context.ScannerEvents.Any(e => e.Id == se.Id));
    }

    [Fact]
    public async Task RemoveAllByScannerIdAsync_RemovesAllEventsForScanner()
    {
        var scanner1 = SeedScanner("SN-A");
        var scanner2 = SeedScanner("SN-B");
        SeedEvent(scanner1.Id);
        SeedEvent(scanner1.Id);
        SeedEvent(scanner2.Id);
        var storage = new RemoveScannerEventStorage(_context);

        await storage.RemoveAllByScannerIdAsync(scanner1.Id, CancellationToken.None);

        Assert.Empty(_context.ScannerEvents.Where(e => e.ScannerId == scanner1.Id));
        Assert.Single(_context.ScannerEvents.Where(e => e.ScannerId == scanner2.Id));
    }
}

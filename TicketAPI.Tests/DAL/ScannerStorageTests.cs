using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.CreateScanner;
using TicketAPI.DAL.Storage.GetScannerById;
using TicketAPI.DAL.Storage.GetScanners;
using TicketAPI.DAL.Storage.UpdateScanner;
using TicketAPI.Domain.Enums;
using TicketAPI.Tests.DAL.Infrastructure;

namespace TicketAPI.Tests.DAL;

[Collection("SqlServer")]
public class ScannerStorageTests : IAsyncLifetime
{
    private readonly SqlServerContainerFixture _fixture;
    private TicketDbContext _context = null!;
    private IDbContextTransaction _transaction = null!;

    public ScannerStorageTests(SqlServerContainerFixture fixture) => _fixture = fixture;

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

    private Scanner Seed(string serial = "SN-001", EScannerStatus status = EScannerStatus.Active)
    {
        var s = new Scanner
        {
            Id = Guid.NewGuid(),
            SerialNumber = serial,
            Status = status,
            CreatedAt = DateTimeOffset.UtcNow
        };
        _context.Scanners.Add(s);
        _context.SaveChanges();
        return s;
    }

    // --- CreateScannerStorage ---

    [Fact]
    public async Task CreateAsync_PersistsScanner_AndReturnsNewId()
    {
        var storage = new CreateScannerStorage(_context);

        var id = await storage.CreateAsync("SN-NEW", EScannerStatus.Active, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        var scanner = _context.Scanners.Single(s => s.Id == id);
        Assert.Equal("SN-NEW", scanner.SerialNumber);
        Assert.Equal(EScannerStatus.Active, scanner.Status);
    }

    // --- GetScannerByIdStorage ---

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        var storage = new GetScannerByIdStorage(_context);

        var result = await storage.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsModel_WhenFound()
    {
        var scanner = Seed("SN-A");
        var storage = new GetScannerByIdStorage(_context);

        var result = await storage.GetByIdAsync(scanner.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(scanner.Id, result.Id);
        Assert.Equal("SN-A", result.SerialNumber);
    }

    [Fact]
    public async Task GetBySerialNumberAsync_ReturnsModel_WhenFound()
    {
        Seed("SN-SERIAL");
        var storage = new GetScannerByIdStorage(_context);

        var result = await storage.GetBySerialNumberAsync("SN-SERIAL", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("SN-SERIAL", result.SerialNumber);
    }

    [Fact]
    public async Task IsExistsAsync_ReturnsTrueAndFalse()
    {
        var scanner = Seed();
        var storage = new GetScannerByIdStorage(_context);

        Assert.True(await storage.IsExistsAsync(scanner.Id, CancellationToken.None));
        Assert.False(await storage.IsExistsAsync(Guid.NewGuid(), CancellationToken.None));
    }

    // --- GetScannersStorage ---

    [Fact]
    public async Task GetScannersAsync_ReturnsAll_WhenNoFilter()
    {
        Seed("SN-1", EScannerStatus.Active);
        Seed("SN-2", EScannerStatus.Inactive);
        Seed("SN-3", EScannerStatus.Active);
        var storage = new GetScannersStorage(_context);

        var result = await storage.GetAsync(1, 10, null, CancellationToken.None);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetScannersAsync_FiltersByStatus()
    {
        Seed("SN-1", EScannerStatus.Active);
        Seed("SN-2", EScannerStatus.Inactive);
        Seed("SN-3", EScannerStatus.Active);
        var storage = new GetScannersStorage(_context);

        var result = await storage.GetAsync(1, 10, EScannerStatus.Active, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Data!, s => Assert.Equal(EScannerStatus.Active, s.Status));
    }

    [Fact]
    public async Task GetScannersAsync_PaginatesCorrectly()
    {
        for (int i = 0; i < 5; i++)
            Seed($"SN-{i:D3}");
        var storage = new GetScannersStorage(_context);

        var result = await storage.GetAsync(2, 2, null, CancellationToken.None);

        Assert.Equal(5, result.Count);
        Assert.Equal(2, result.Data!.Count);
    }

    // --- UpdateScannerStorage ---

    [Fact]
    public async Task UpdateStatusAsync_ChangesStatus()
    {
        var scanner = Seed("SN-UPD", EScannerStatus.Active);
        var storage = new UpdateScannerStorage(_context);

        await storage.UpdateStatusAsync(scanner.Id, EScannerStatus.Inactive, CancellationToken.None);

        var updated = _context.Scanners.Find(scanner.Id)!;
        Assert.Equal(EScannerStatus.Inactive, updated.Status);
        Assert.NotNull(updated.UpdatedAt);
    }
}

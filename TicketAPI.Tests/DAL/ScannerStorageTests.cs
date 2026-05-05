using Microsoft.EntityFrameworkCore;
using TicketAPI.Domain.Enums;
using TicketAPI.DAL;
using TicketAPI.DAL.Storage.CreateScanner;
using TicketAPI.DAL.Storage.GetScannerById;
using TicketAPI.DAL.Storage.GetScanners;
using TicketAPI.DAL.Storage.UpdateScanner;
using TicketAPI.DAL.Entities;

namespace TicketAPI.Tests.DAL;

public class ScannerStorageTests
{
    private static TicketDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TicketDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TicketDbContext(options);
    }

    private static Scanner Seed(TicketDbContext ctx, string serial = "SN-001", EScannerStatus status = EScannerStatus.Active)
    {
        var s = new Scanner
        {
            Id = Guid.NewGuid(),
            SerialNumber = serial,
            Status = status,
            CreatedAt = DateTimeOffset.UtcNow
        };
        ctx.Scanners.Add(s);
        ctx.SaveChanges();
        return s;
    }

    // --- CreateScannerStorage ---

    [Fact]
    public async Task CreateAsync_PersistsScanner_AndReturnsNewId()
    {
        using var ctx = CreateContext();
        var storage = new CreateScannerStorage(ctx);

        var id = await storage.CreateAsync("SN-NEW", EScannerStatus.Active, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        var scanner = ctx.Scanners.Single(s => s.Id == id);
        Assert.Equal("SN-NEW", scanner.SerialNumber);
        Assert.Equal(EScannerStatus.Active, scanner.Status);
    }

    // --- GetScannerByIdStorage ---

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx = CreateContext();
        var storage = new GetScannerByIdStorage(ctx);

        var result = await storage.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsModel_WhenFound()
    {
        using var ctx = CreateContext();
        var scanner = Seed(ctx, "SN-A");
        var storage = new GetScannerByIdStorage(ctx);

        var result = await storage.GetByIdAsync(scanner.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(scanner.Id, result.Id);
        Assert.Equal("SN-A", result.SerialNumber);
    }

    [Fact]
    public async Task GetBySerialNumberAsync_ReturnsModel_WhenFound()
    {
        using var ctx = CreateContext();
        Seed(ctx, "SN-SERIAL");
        var storage = new GetScannerByIdStorage(ctx);

        var result = await storage.GetBySerialNumberAsync("SN-SERIAL", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("SN-SERIAL", result.SerialNumber);
    }

    [Fact]
    public async Task IsExistsAsync_ReturnsTrueAndFalse()
    {
        using var ctx = CreateContext();
        var scanner = Seed(ctx);
        var storage = new GetScannerByIdStorage(ctx);

        Assert.True(await storage.IsExistsAsync(scanner.Id, CancellationToken.None));
        Assert.False(await storage.IsExistsAsync(Guid.NewGuid(), CancellationToken.None));
    }

    // --- GetScannersStorage ---

    [Fact]
    public async Task GetScannersAsync_ReturnsAll_WhenNoFilter()
    {
        using var ctx = CreateContext();
        Seed(ctx, "SN-1", EScannerStatus.Active);
        Seed(ctx, "SN-2", EScannerStatus.Inactive);
        Seed(ctx, "SN-3", EScannerStatus.Active);
        var storage = new GetScannersStorage(ctx);

        var result = await storage.GetAsync(1, 10, null, CancellationToken.None);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetScannersAsync_FiltersByStatus()
    {
        using var ctx = CreateContext();
        Seed(ctx, "SN-1", EScannerStatus.Active);
        Seed(ctx, "SN-2", EScannerStatus.Inactive);
        Seed(ctx, "SN-3", EScannerStatus.Active);
        var storage = new GetScannersStorage(ctx);

        var result = await storage.GetAsync(1, 10, EScannerStatus.Active, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Data!, s => Assert.Equal(EScannerStatus.Active, s.Status));
    }

    [Fact]
    public async Task GetScannersAsync_PaginatesCorrectly()
    {
        using var ctx = CreateContext();
        for (int i = 0; i < 5; i++)
            Seed(ctx, $"SN-{i:D3}");
        var storage = new GetScannersStorage(ctx);

        var result = await storage.GetAsync(2, 2, null, CancellationToken.None);

        Assert.Equal(5, result.Count);
        Assert.Equal(2, result.Data!.Count);
    }

    // --- UpdateScannerStorage ---

    [Fact]
    public async Task UpdateStatusAsync_ChangesStatus()
    {
        using var ctx = CreateContext();
        var scanner = Seed(ctx, "SN-UPD", EScannerStatus.Active);
        var storage = new UpdateScannerStorage(ctx);

        await storage.UpdateStatusAsync(scanner.Id, EScannerStatus.Inactive, CancellationToken.None);

        var updated = ctx.Scanners.Find(scanner.Id)!;
        Assert.Equal(EScannerStatus.Inactive, updated.Status);
        Assert.NotNull(updated.UpdatedAt);
    }
}

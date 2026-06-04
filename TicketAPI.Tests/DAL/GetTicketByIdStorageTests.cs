using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.GetTicketById;
using TicketAPI.Domain.Enums;
using TicketAPI.Tests.DAL.Infrastructure;

namespace TicketAPI.Tests.DAL;

[Collection("SqlServer")]
public class GetTicketByIdStorageTests : IAsyncLifetime
{
    private readonly SqlServerContainerFixture _fixture;
    private TicketDbContext _context = null!;
    private IDbContextTransaction _transaction = null!;

    public GetTicketByIdStorageTests(SqlServerContainerFixture fixture) => _fixture = fixture;

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

    private Ticket Seed(string qrCode = "QR-1", ETicketStatus status = ETicketStatus.Unused)
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
        _context.Tickets.Add(t);
        _context.SaveChanges();
        return t;
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        var storage = new GetTicketByIdStorage(_context);

        var result = await storage.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDetailModel_WhenFound()
    {
        var ticket = Seed("QR-TEST");
        var storage = new GetTicketByIdStorage(_context);

        var result = await storage.GetByIdAsync(ticket.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(ticket.Id, result.Id);
        Assert.Equal("QR-TEST", result.QRCode);
        Assert.Empty(result.Validations);
    }

    [Fact]
    public async Task GetByQrCodeAsync_ReturnsNull_WhenNotFound()
    {
        var storage = new GetTicketByIdStorage(_context);

        var result = await storage.GetByQrCodeAsync("NONEXISTENT", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByQrCodeAsync_ReturnsModel_WhenFound()
    {
        Seed("QR-ABC");
        var storage = new GetTicketByIdStorage(_context);

        var result = await storage.GetByQrCodeAsync("QR-ABC", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("QR-ABC", result.QRCode);
    }

    [Fact]
    public async Task IsExistsAsync_ReturnsFalse_WhenNotFound()
    {
        var storage = new GetTicketByIdStorage(_context);

        var result = await storage.IsExistsAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task IsExistsAsync_ReturnsTrue_WhenFound()
    {
        var ticket = Seed();
        var storage = new GetTicketByIdStorage(_context);

        var result = await storage.IsExistsAsync(ticket.Id, CancellationToken.None);

        Assert.True(result);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.CreateTicketValidation;
using TicketAPI.DAL.Storage.GetTicketValidations;
using TicketAPI.Domain.Enums;
using TicketAPI.Tests.DAL.Infrastructure;

namespace TicketAPI.Tests.DAL;

[Collection("SqlServer")]
public class TicketValidationStorageTests : IAsyncLifetime
{
    private readonly SqlServerContainerFixture _fixture;
    private TicketDbContext _context = null!;
    private IDbContextTransaction _transaction = null!;

    public TicketValidationStorageTests(SqlServerContainerFixture fixture) => _fixture = fixture;

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

    private Ticket SeedTicket()
    {
        var t = new Ticket
        {
            Id = Guid.NewGuid(),
            OrderItemId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            QRCode = Guid.NewGuid().ToString(),
            Status = ETicketStatus.Unused,
            CreatedAt = DateTimeOffset.UtcNow
        };
        _context.Tickets.Add(t);
        _context.SaveChanges();
        return t;
    }

    private Scanner SeedScanner()
    {
        var s = new Scanner { Id = Guid.NewGuid(), SerialNumber = Guid.NewGuid().ToString(), Status = EScannerStatus.Active, CreatedAt = DateTimeOffset.UtcNow };
        _context.Scanners.Add(s);
        _context.SaveChanges();
        return s;
    }

    // --- CreateTicketValidationStorage ---

    [Fact]
    public async Task CreateAsync_PersistsValidation_AndReturnsNewId()
    {
        var ticket = SeedTicket();
        var scanner = SeedScanner();
        var storage = new CreateTicketValidationStorage(_context);
        var validationTime = DateTimeOffset.UtcNow;
        var scannedBy = Guid.NewGuid();

        var id = await storage.CreateAsync(ticket.Id, scanner.Id, scannedBy, "inspector@example.com", validationTime, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        var validation = _context.TicketValidations.Single(v => v.Id == id);
        Assert.Equal(ticket.Id, validation.TicketId);
        Assert.Equal(scanner.Id, validation.ScannerId);
        Assert.Equal(scannedBy, validation.ScannedBy);
        Assert.Equal("inspector@example.com", validation.ValidatedBy);
        Assert.Equal(validationTime, validation.ValidationTime);
    }

    [Fact]
    public async Task CreateAsync_AllowsNullScannerId()
    {
        var ticket = SeedTicket();
        var storage = new CreateTicketValidationStorage(_context);

        var id = await storage.CreateAsync(ticket.Id, null, Guid.NewGuid(), "manual", DateTimeOffset.UtcNow, CancellationToken.None);

        var validation = _context.TicketValidations.Single(v => v.Id == id);
        Assert.Null(validation.ScannerId);
    }

    // --- GetTicketValidationsStorage ---

    [Fact]
    public async Task GetAsync_FiltersByTicketId()
    {
        var ticket1 = SeedTicket();
        var ticket2 = SeedTicket();
        var create = new CreateTicketValidationStorage(_context);
        await create.CreateAsync(ticket1.Id, null, Guid.NewGuid(), "user1", DateTimeOffset.UtcNow, CancellationToken.None);
        await create.CreateAsync(ticket1.Id, null, Guid.NewGuid(), "user2", DateTimeOffset.UtcNow, CancellationToken.None);
        await create.CreateAsync(ticket2.Id, null, Guid.NewGuid(), "user3", DateTimeOffset.UtcNow, CancellationToken.None);

        var storage = new GetTicketValidationsStorage(_context);
        var result = await storage.GetAsync(1, 10, ticket1.Id, null, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Data!, v => Assert.Equal(ticket1.Id, v.TicketId));
    }

    [Fact]
    public async Task GetAsync_FiltersByScannerId()
    {
        var ticket = SeedTicket();
        var scanner = SeedScanner();
        var create = new CreateTicketValidationStorage(_context);
        await create.CreateAsync(ticket.Id, scanner.Id, Guid.NewGuid(), "user1", DateTimeOffset.UtcNow, CancellationToken.None);
        await create.CreateAsync(ticket.Id, null, Guid.NewGuid(), "user2", DateTimeOffset.UtcNow, CancellationToken.None);

        var storage = new GetTicketValidationsStorage(_context);
        var result = await storage.GetAsync(1, 10, null, scanner.Id, CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Equal(scanner.Id, result.Data![0].ScannerId);
    }

    [Fact]
    public async Task GetAsync_PaginatesCorrectly()
    {
        var ticket = SeedTicket();
        var create = new CreateTicketValidationStorage(_context);
        for (int i = 0; i < 5; i++)
            await create.CreateAsync(ticket.Id, null, Guid.NewGuid(), $"user{i}", DateTimeOffset.UtcNow, CancellationToken.None);

        var storage = new GetTicketValidationsStorage(_context);
        var result = await storage.GetAsync(1, 3, ticket.Id, null, CancellationToken.None);

        Assert.Equal(5, result.Count);
        Assert.Equal(3, result.Data!.Count);
    }
}

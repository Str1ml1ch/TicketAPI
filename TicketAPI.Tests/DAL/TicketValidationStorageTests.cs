using Microsoft.EntityFrameworkCore;
using TicketAPI.Domain.Enums;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.CreateTicketValidation;
using TicketAPI.DAL.Storage.GetTicketValidations;

namespace TicketAPI.Tests.DAL;

public class TicketValidationStorageTests
{
    private static TicketDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TicketDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TicketDbContext(options);
    }

    private static Ticket SeedTicket(TicketDbContext ctx)
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
        ctx.Tickets.Add(t);
        ctx.SaveChanges();
        return t;
    }

    private static Scanner SeedScanner(TicketDbContext ctx)
    {
        var s = new Scanner { Id = Guid.NewGuid(), SerialNumber = Guid.NewGuid().ToString(), Status = EScannerStatus.Active, CreatedAt = DateTimeOffset.UtcNow };
        ctx.Scanners.Add(s);
        ctx.SaveChanges();
        return s;
    }

    // --- CreateTicketValidationStorage ---

    [Fact]
    public async Task CreateAsync_PersistsValidation_AndReturnsNewId()
    {
        using var ctx = CreateContext();
        var ticket = SeedTicket(ctx);
        var scanner = SeedScanner(ctx);
        var storage = new CreateTicketValidationStorage(ctx);
        var validationTime = DateTimeOffset.UtcNow;
        var scannedBy = Guid.NewGuid();

        var id = await storage.CreateAsync(ticket.Id, scanner.Id, scannedBy, "inspector@example.com", validationTime, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        var validation = ctx.TicketValidations.Single(v => v.Id == id);
        Assert.Equal(ticket.Id, validation.TicketId);
        Assert.Equal(scanner.Id, validation.ScannerId);
        Assert.Equal(scannedBy, validation.ScannedBy);
        Assert.Equal("inspector@example.com", validation.ValidatedBy);
        Assert.Equal(validationTime, validation.ValidationTime);
    }

    [Fact]
    public async Task CreateAsync_AllowsNullScannerId()
    {
        using var ctx = CreateContext();
        var ticket = SeedTicket(ctx);
        var storage = new CreateTicketValidationStorage(ctx);

        var id = await storage.CreateAsync(ticket.Id, null, Guid.NewGuid(), "manual", DateTimeOffset.UtcNow, CancellationToken.None);

        var validation = ctx.TicketValidations.Single(v => v.Id == id);
        Assert.Null(validation.ScannerId);
    }

    // --- GetTicketValidationsStorage ---

    [Fact]
    public async Task GetAsync_FiltersByTicketId()
    {
        using var ctx = CreateContext();
        var ticket1 = SeedTicket(ctx);
        var ticket2 = SeedTicket(ctx);
        var storage2 = new CreateTicketValidationStorage(ctx);
        await storage2.CreateAsync(ticket1.Id, null, Guid.NewGuid(), "user1", DateTimeOffset.UtcNow, CancellationToken.None);
        await storage2.CreateAsync(ticket1.Id, null, Guid.NewGuid(), "user2", DateTimeOffset.UtcNow, CancellationToken.None);
        await storage2.CreateAsync(ticket2.Id, null, Guid.NewGuid(), "user3", DateTimeOffset.UtcNow, CancellationToken.None);

        var storage = new GetTicketValidationsStorage(ctx);
        var result = await storage.GetAsync(1, 10, ticket1.Id, null, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Data!, v => Assert.Equal(ticket1.Id, v.TicketId));
    }

    [Fact]
    public async Task GetAsync_FiltersByScannerId()
    {
        using var ctx = CreateContext();
        var ticket = SeedTicket(ctx);
        var scanner = SeedScanner(ctx);
        var create = new CreateTicketValidationStorage(ctx);
        await create.CreateAsync(ticket.Id, scanner.Id, Guid.NewGuid(), "user1", DateTimeOffset.UtcNow, CancellationToken.None);
        await create.CreateAsync(ticket.Id, null, Guid.NewGuid(), "user2", DateTimeOffset.UtcNow, CancellationToken.None);

        var storage = new GetTicketValidationsStorage(ctx);
        var result = await storage.GetAsync(1, 10, null, scanner.Id, CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Equal(scanner.Id, result.Data![0].ScannerId);
    }

    [Fact]
    public async Task GetAsync_PaginatesCorrectly()
    {
        using var ctx = CreateContext();
        var ticket = SeedTicket(ctx);
        var create = new CreateTicketValidationStorage(ctx);
        for (int i = 0; i < 5; i++)
            await create.CreateAsync(ticket.Id, null, Guid.NewGuid(), $"user{i}", DateTimeOffset.UtcNow, CancellationToken.None);

        var storage = new GetTicketValidationsStorage(ctx);
        var result = await storage.GetAsync(1, 3, ticket.Id, null, CancellationToken.None);

        Assert.Equal(5, result.Count);
        Assert.Equal(3, result.Data!.Count);
    }
}

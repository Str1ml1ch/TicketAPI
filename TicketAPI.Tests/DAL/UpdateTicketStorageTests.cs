using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TicketAPI.DAL;
using TicketAPI.DAL.Entities;
using TicketAPI.DAL.Storage.UpdateTicket;
using TicketAPI.Domain.Enums;
using TicketAPI.Tests.DAL.Infrastructure;

namespace TicketAPI.Tests.DAL;

[Collection("SqlServer")]
public class UpdateTicketStorageTests : IAsyncLifetime
{
    private readonly SqlServerContainerFixture _fixture;
    private TicketDbContext _context = null!;
    private IDbContextTransaction _transaction = null!;

    public UpdateTicketStorageTests(SqlServerContainerFixture fixture) => _fixture = fixture;

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

    private Ticket Seed()
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
        _context.Tickets.Add(t);
        _context.SaveChanges();
        return t;
    }

    [Fact]
    public async Task UpdateStatusAsync_ChangesStatus()
    {
        var ticket = Seed();
        var storage = new UpdateTicketStorage(_context);
        var usedAt = DateTimeOffset.UtcNow;

        await storage.UpdateStatusAsync(ticket.Id, ETicketStatus.Used, usedAt, CancellationToken.None);

        var updated = _context.Tickets.Find(ticket.Id)!;
        Assert.Equal(ETicketStatus.Used, updated.Status);
        Assert.Equal(usedAt, updated.UsedAt);
        Assert.NotNull(updated.UpdatedAt);
    }

    [Fact]
    public async Task UpdateStatusAsync_ToCancelled_SetsNullUsedAt()
    {
        var ticket = Seed();
        var storage = new UpdateTicketStorage(_context);

        await storage.UpdateStatusAsync(ticket.Id, ETicketStatus.Cancelled, null, CancellationToken.None);

        var updated = _context.Tickets.Find(ticket.Id)!;
        Assert.Equal(ETicketStatus.Cancelled, updated.Status);
        Assert.Null(updated.UsedAt);
    }
}

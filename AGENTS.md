# TicketAPI — Agent Instructions

## Architecture

Three-project layered solution with strict dependency rules:

```
TicketAPI (Web) → TicketAPI.Domain ← TicketAPI.DAL
```

- **`TicketAPI`** — ASP.NET Core 8 Web API, controllers, middleware, JWT auth, Redis cache registration
- **`TicketAPI.Domain`** — Business logic, MediatR use cases, storage interfaces, models, enums, domain exceptions
- **`TicketAPI.DAL`** — EF Core 8 (SQL Server), entity configurations, storage implementations, Specification pattern

The Domain layer defines storage interfaces (`IGetTicketsStorage`, `ICreateTicketStorage`, etc.); the DAL implements them. Controllers never touch the DAL directly.

## Request Flow

```
Controller → IMediator.Send(Request) → UseCase Handler (Domain) → IXxxStorage interface → XxxStorage (DAL) → TicketDbContext
```

## Build & Test

```bash
dotnet build TicketAPI.sln
dotnet test TicketAPI.Tests/TicketAPI.Tests.csproj

# EF migrations (always target from solution root)
dotnet ef migrations add <Name> --project TicketAPI.DAL --startup-project TicketAPI
dotnet ef database update --project TicketAPI.DAL --startup-project TicketAPI
```

## Conventions

### Use Cases (Domain)
- Each operation lives in `TicketAPI.Domain/UseCases/<Verb><Entity>/` with two files:
  - `<UseCase>Request.cs` — implements `IRequest<TResponse>`
  - `<UseCase>Request.Handler.cs` — implements `IRequestHandler<TRequest, TResponse>`, injects `IXxxStorage`
- Existing use cases: `CancelTicket`, `CreateScanner`, `CreateScannerEvent`, `CreateTicket`, `GetScannerById`, `GetScannerEvents`, `GetScanners`, `GetTicketById`, `GetTickets`, `GetTicketValidations`, `RemoveScanner`, `RemoveScannerEvent`, `UpdateScannerStatus`, `ValidateTicket`

### Storage (Interface Segregation, not generic repository)
- Domain: `TicketAPI.Domain/Storage/<Operation>/I<Operation>Storage.cs`
- DAL: `TicketAPI.DAL/Storage/<Operation>/<Operation>Storage.cs`
- Each interface is operation-scoped (e.g., `IGetTicketsStorage`, `ICreateTicketStorage`) — no `IRepository<T>`

### Specification Pattern
- Specs live in `TicketAPI.DAL/Specifications/<Entity>/`
- Implement `ISpecification<T>` with a single `ToExpression()` returning `Expression<Func<T, bool>>`
- Applied via `.Where(spec.ToExpression())` in storage classes

### Naming
- Models (Domain DTOs): `<Entity>Model`, `<Entity>DetailModel`
- Enums: prefix `E` (e.g., `ETicketStatus`, `EScannerStatus`)
- Domain exceptions: extend `NotFoundException` (base in `TicketAPI.Domain/Exceptions/`)

### EF Core
- All entity configurations are `IEntityTypeConfiguration<T>` classes in `TicketAPI.DAL/Configurations/` — auto-discovered via `ApplyConfigurationsFromAssembly`
- Enum columns: `.HasConversion<string>().HasMaxLength(50)`
- Entities extend `BaseDbEntity` (from `Homework.Ticketing.System.Shared`) providing `Id`, `CreatedAt`, `UpdatedAt`
- Return types wrap results in `ResultModel<T>` (also from the shared library)

### Error Handling
- Throw domain exceptions (e.g., `TicketNotFoundException`) in handlers when entities are not found
- `ExceptionHandlerMiddleware` maps `NotFoundException` → HTTP 404, all others → HTTP 500

### Auth
- JWT Bearer auth is required on all endpoints (`[Authorize]`)
- User identity extracted from `ClaimTypes.NameIdentifier` and `ClaimTypes.Name`

## Testing
- Tests are in `TicketAPI.Tests/DAL/` and test **storage classes directly** using `Microsoft.EntityFrameworkCore.InMemory`
- Each test creates an isolated in-memory DB: `new DbContextOptionsBuilder<TicketDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())`
- Seed data directly in the `DbContext`, instantiate storage class, then assert on results
- `Moq` is available for future handler/use-case unit tests (mock the storage interfaces)

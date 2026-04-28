# TodoApi

This project is a clean Layered ASP.NET Core Web API for managing tasks, using DTOs, FluentValidation, async service methods, and centralized error handling.

## Design

- `Controllers`: receive HTTP requests and delegate work to the service layer.
- `Services`: contain business logic and data mapping, keeping controllers thin.
- `DTOs`: separate request and response payloads from domain models.
- `Validators`: FluentValidation rules ensure request DTOs are valid before controller actions execute.
- `Middleware`: centralized exception handling returns standardized ProblemDetails responses.
- `Dependency Injection`: `TaskService` is registered as `Scoped`, and the in-memory `TaskStore` is registered as `Singleton` to preserve shared data across requests.

## Production-Ready Implementation

### Thread-Safety
- `InMemoryTaskStore` uses `ConcurrentDictionary<int, TaskItem>` for thread-safe concurrent access.
- `TaskService` uses `Interlocked.Increment()` to generate unique task IDs safely in multi-threaded scenarios.
- All add/update/remove operations use thread-safe methods (`TryAdd`, `TryGetValue`, `TryRemove`).

### Validation
- `TaskCreateDtoValidator` and `TaskUpdateDtoValidator` enforce:
  - `Title` is required and must not exceed 200 characters
  - `Description` is optional but must not exceed 1000 characters if provided
- Invalid DTOs return HTTP 400 with `ValidationProblemDetails` in the response body.

### Async Implementation
- All service methods return properly typed `Task<T>` using `Task.FromResult()` for non-blocking synchronous operations.
- Avoids fake async (`await Task.CompletedTask`) which was misleading.

## Endpoints

- `GET /tasks`
- `GET /tasks/{id}`
- `POST /tasks`
- `PUT /tasks/{id}`
- `DELETE /tasks/{id}`

## Data Models

- `TaskItem` (domain model)
  - `Id` (int)
  - `Title` (string)
  - `Description` (string)
  - `IsCompleted` (bool)
  - `CreatedAt` (DateTime)

- `TaskCreateDto` / `TaskUpdateDto`
  - `Title` is required (NotEmpty, MaxLength 200)
  - `Description` is optional (MaxLength 1000)
  - `IsCompleted` is optional

- `TaskReadDto` is returned by API responses instead of exposing `TaskItem` directly.

## Dependency Injection

- `AddScoped<ITaskService, TaskService>()`
- `AddSingleton<ITaskStore, InMemoryTaskStore>()`

This means:
- each request gets its own `TaskService` instance
- the shared in-memory task store is preserved across requests with thread-safe operations

## Development

1. Restore packages:
   ```bash
   dotnet restore
   ```

2. Build:
   ```bash
   dotnet build
   ```

3. Run:
   ```bash
   dotnet run
   ```

## Notes

- `TaskService` sets `CreatedAt` with `DateTime.UtcNow` when new tasks are created.
- Controllers are intentionally thin and do not contain business logic.
- Error handling is centralized in `ErrorHandlingMiddleware`.
- The store uses `ConcurrentDictionary` instead of `List` for thread-safety in high-concurrency scenarios.

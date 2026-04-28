# TodoApi

This project is a clean Layered ASP.NET Core Web API for managing tasks, using DTOs, FluentValidation, async service methods, and centralized error handling.

## Design

- `Controllers`: receive HTTP requests and delegate work to the service layer.
- `Services`: contain business logic and data mapping, keeping controllers thin.
- `DTOs`: separate request and response payloads from domain models.
- `Validators`: FluentValidation rules ensure request DTOs are valid before controller actions execute.
- `Middleware`: centralized exception handling returns standardized ProblemDetails responses.
- `Dependency Injection`: `TaskService` is registered as `Scoped`, and the in-memory `TaskStore` is registered as `Singleton` to preserve shared data across requests.

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
  - `Title` is required and validated with FluentValidation
  - `Description` is optional
  - `IsCompleted` is optional

- `TaskReadDto` is returned by API responses instead of exposing `TaskItem` directly.

## Validation

- `TaskCreateDtoValidator` and `TaskUpdateDtoValidator` enforce:
  - `Title` must not be empty
- Invalid DTOs return HTTP 400 with `ValidationProblemDetails` in the response body.

## Dependency Injection

- `AddScoped<ITaskService, TaskService>()`
- `AddSingleton<ITaskStore, InMemoryTaskStore>()`

This means:
- each request gets its own `TaskService`
- the shared in-memory task list is preserved across requests

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

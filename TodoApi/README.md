# TodoApi

Simple ASP.NET Core Web API for managing tasks with layered architecture, DTOs, validation, async operations, and centralized error handling.

## Endpoints

- `GET /tasks`
- `GET /tasks/{id}`
- `POST /tasks`
- `PUT /tasks/{id}`
- `DELETE /tasks/{id}`

## Models

- `TaskItem`
  - `Id` (int)
  - `Title` (string)
  - `Description` (string)
  - `IsCompleted` (bool)
  - `CreatedAt` (DateTime)

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

- Validation is handled by FluentValidation.
- Error handling is centralized in middleware.
- The API uses DTOs for input/output instead of exposing domain models directly.

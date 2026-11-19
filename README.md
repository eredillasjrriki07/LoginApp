# LoginApp

A simple authentication and authorization API built with **ASP.NET Core**, following **Clean Architecture** and **DDD** principles.

## Features

- User registration and login
- Password hashing with **BCrypt**
- **JWT** access tokens (short-lived)
- **Refresh tokens** with rotation and reuse detection
- Role-based **permissions** (`[HasPermission("Users.View")]`)
- Value objects (`Email`, `Username`) with validation
- Domain events
- Global exception handling
- FluentValidation for request DTOs

## Tech Stack

- .NET / ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- BCrypt.Net-Next
- FluentValidation
- JWT Bearer authentication

## Project Structure

```
LoginApp.Domain          Entities, value objects, domain events, permission constants
LoginApp.Application     Use cases/services, DTOs, validators, abstractions (interfaces)
LoginApp.Infrastructure  EF Core, repositories, BCrypt, JWT provider, refresh token generator
LoginApp.Api             Controllers, authorization (attribute, handler, policy provider), Program.cs
```

Dependencies point inward only: `Api -> Application -> Domain`, and `Infrastructure -> Application`.
Domain references nothing.

## Getting Started

### 1. Prerequisites

- .NET SDK
- PostgreSQL
- EF Core tools (`dotnet tool install --global dotnet-ef`)

### 2. Configuration

`appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=loginappdb;Username=postgres;Password=<your-password>"
  },
  "Jwt": {
    "Issuer": "LoginApp",
    "Audience": "LoginApp",
    "ExpirationInMinutes": 15
  }
}
```

Keep secrets out of source control. For development, use user-secrets:

```bash
dotnet user-secrets set "Jwt:Secret" "a-long-random-string-at-least-32-chars" --project LoginApp.Api
dotnet user-secrets set "Seed:AdminPassword" "<a-strong-password>" --project LoginApp.Api
```

### 3. Database

```bash
dotnet ef database update --project LoginApp.Infrastructure --startup-project LoginApp.Api
```

To add a migration:

```bash
dotnet ef migrations add <Name> --project LoginApp.Infrastructure --startup-project LoginApp.Api
```

### 4. Run

```bash
dotnet run --project LoginApp.Api
```

## API Endpoints

### Auth

| Method | Route | Access | Description |
|---|---|---|---|
| POST | `/api/auth/register` | Anonymous | Create an account |
| POST | `/api/auth/login` | Anonymous | Returns access and refresh tokens |
| POST | `/api/auth/refresh` | Anonymous | Exchanges a refresh token for a new pair |
| POST | `/api/auth/logout` | Authenticated | Revokes the refresh token |

### Users

| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/api/users/{id}` | Owner or `Users.View` | Get a user by id |
| PATCH | `/api/users/{id}` | `Users.Update` | Update a user |
| DELETE | `/api/users/{id}` | `Users.Delete` | Delete a user |

Send the access token as `Authorization: Bearer <token>`.

## Authentication Flow

1. **Login** returns an access token (15 min) and a refresh token (7 days).
2. When the access token expires, the client calls **refresh** with its refresh token.
3. Every refresh issues a **new pair** and revokes the old refresh token (rotation).
4. If a revoked refresh token is used again, all of that user's refresh tokens are revoked (reuse detection).
5. Only the **hash** of a refresh token is stored in the database.

## Roles and Permissions

Permissions are stored in the database and added to the JWT as `permission` claims at login and refresh.

```
Users ──< UserRoles >── Roles ──< RolePermissions >── Permissions
```

| Role | Permissions |
|---|---|
| Admin | Users.View, Users.Create, Users.Update, Users.Delete |
| Viewer | Users.View |

New registrations get the **Viewer** role. An Admin account is seeded at startup using `Seed:AdminPassword`.

Protect an endpoint with:

```csharp
[HasPermission(Permissions.UsersDelete)]
[HttpDelete("{id:guid}")]
public async Task<IActionResult> Delete(Guid id, CancellationToken ct) { ... }
```

Results: no token returns **401**, a token without the permission returns **403**.

## Security Notes

- Passwords are validated by `PasswordPolicy`, then hashed with BCrypt. Plain passwords never reach the domain.
- Login returns the same error for an unknown email and a wrong password.
- Never log passwords or tokens.
- Permissions in an access token stay valid until it expires. Refresh reloads them.
- Unique indexes on `Email` and `Username` guard against duplicate registrations.

## Testing

```bash
dotnet test
```

Includes a unit test for `PermissionHandler` and an integration test verifying that a Viewer gets **403** on `DELETE /api/users/{id}`. Use a separate test database, not your development one.

## License

For learning and demo purposes.

# Educonnect

A social educational platform where students can connect, share knowledge, and collaborate through posts, comments, reactions, and groups.

## Tech Stack

- **Runtime** — .NET 8
- **Framework** — ASP.NET Core Web API
- **ORM** — Entity Framework Core 8
- **Database** — SQL Server
- **Auth** — ASP.NET Core Identity + JWT (access token + refresh token)
- **Docs** — Swagger / OpenAPI

## Architecture

The solution follows a layered architecture with clear separation of concerns.

```
Educonnect/
├── Educonnect              # Web API — controllers, middleware, entry point
├── Educonnect.Domain       # Entities, interfaces, enums
├── Educonnect.Application  # Business logic, services, DTOs
├── Educonnect.Infrastructure  # EF Core, DbContext, repositories, migrations
└── Educonnect.Common       # Shared exceptions and enums
```

Each layer only knows about the layer directly below it. The domain has zero dependencies on any framework.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local or remote)
- [EF Core CLI tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

```bash
dotnet tool install --global dotnet-ef
```

### Setup

**1. Clone the repo**

```bash
git clone https://github.com/your-username/educonnect.git
cd educonnect
```

**2. Configure your environment**

Copy the example config and fill in your values:

```bash
cp Educonnect/appsettings.Example.json Educonnect/appsettings.json
```

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=Educonnect;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-secret-key-min-32-characters-long",
    "Issuer": "educonnect",
    "Audience": "educonnect",
    "AccessTokenExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 7
  }
}
```

**3. Apply migrations**

```bash
dotnet ef database update --project Educonnect.Infrastructure --startup-project Educonnect
```

**4. Run**

```bash
dotnet run --project Educonnect
```

API is available at `http://localhost:5065` — Swagger UI at `http://localhost:5065/swagger`.

## Authentication

Educonnect uses a **JWT + Refresh Token** strategy.

| Endpoint | Description |
|---|---|
| `POST /auth/signup` | Create a new account |
| `POST /auth/signin` | Sign in, receive access + refresh token |
| `POST /auth/refresh` | Rotate refresh token, get new access token |
| `POST /auth/signout` | Revoke refresh token server-side |

- **Access token** — short-lived JWT (15 min), sent in the `Authorization: Bearer` header
- **Refresh token** — long-lived opaque token (7 days), stored in the database with rotation on every use

## Domain Model

```
User
 └── Profile
      ├── Posts
      │    ├── Comments
      │    │    └── Reactions
      │    └── Reactions
      └── Groups (many-to-many)
```

All main entities support **soft delete** — records are never hard-deleted, just flagged with `IsDeleted` and automatically filtered out of all queries.

## Project Structure

```
Educonnect.Domain/
├── Entities/       User, Profile, Post, Comment, Group, Reaction, RefreshToken
├── Enums/          Role, ReactionType
└── Interfaces/     IDeletable

Educonnect.Application/
├── Dtos/           Request and response DTOs
└── Services/       IAuthService, ITokenService and their implementations

Educonnect.Infrastructure/
├── Data/           ApplicationDbContext
├── Migrations/
└── Repositories/   Generic repository + entity-specific repositories

Educonnect.Common/
├── Enums/          MessageCodes
└── Exceptions/     BaseException, EntityNotFoundException
```

## Environment Variables

| Key | Description |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `Jwt:Key` | Secret key for signing JWT (min 32 chars) |
| `Jwt:Issuer` | Token issuer identifier |
| `Jwt:Audience` | Token audience identifier |
| `Jwt:AccessTokenExpiryMinutes` | Access token lifetime (default: 15) |
| `Jwt:RefreshTokenExpiryDays` | Refresh token lifetime (default: 7) |

> Never commit `appsettings.json` — use `appsettings.Example.json` as the template.

## Contributing

1. Fork the repo
2. Create a feature branch — `git checkout -b feat/your-feature`
3. Commit your changes
4. Open a pull request

---

Built with .NET 8

# SKB.VideoMultiplexer.Http

HTTP API host for the SKB.VideoMultiplexer. Built with .NET 9 minimal APIs.

## Authentication (Keycloak / OpenID Connect)

The API uses Keycloak for authentication via OpenID Connect. Authentication is configured in `Extensions/AuthExtensions.cs` using a combined scheme:

- **JWT Bearer** token (via `Authorization: Bearer` header) — used for API clients
- **Cookie** authentication — used for browser-based flows
- **OpenID Connect** code flow — used as the default challenge scheme

### Prerequisites

Start Keycloak and PostgreSQL via Docker Compose:

```bash
cd ../../SKB.VideoMultiplexer.Deploy
bash 00_create_env.sh
docker compose up -d
```

### Configuration

Settings are loaded from `appsettings.json` and `dotnet user-secrets`:

| Key | Description | Default |
|---|---|---|
| `Authentication:Schemes:OpenIdConnect:Authority` | Keycloak realm URL | `http://localhost:8080/realms/skb-videomultiplexer` |
| `Authentication:Schemes:OpenIdConnect:ClientId` | OIDC client ID | `skb-videomultiplexer` |
| `Authentication:Schemes:OpenIdConnect:ClientSecret` | OIDC client secret | *(auto-generated)* |
| `Authentication:Schemes:OpenIdConnect:RequireHttpsMetadata` | Disable HTTPS for local dev | `false` |

### Getting an Access Token

Use the Resource Owner Password Credentials (ROPC) flow to obtain a JWT token:

```bash
curl -X POST "http://localhost:8080/realms/skb-videomultiplexer/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=skb-videomultiplexer" \
  -d "username=user" \
  -d "password=user" \
  -d "grant_type=password" \
  -d "scope=openid" | jq
```

The response contains an `access_token` field. Copy this value to authenticate API requests.

### Calling Protected Endpoints

```bash
curl -H "Authorization: Bearer <access_token>" http://localhost:5000/users
```

## Swagger UI

Swagger is enabled in development mode via `Swashbuckle.AspNetCore` (see `Extensions/SwaggerExtensions.cs`).

### Accessing Swagger UI

Navigate to `http://localhost:5000/swagger` in your browser.

### Authenticating via Swagger UI

1. Open Swagger UI at `/swagger`
2. Click the **Authorize** button
3. Paste your `access_token` into the `Value` field (no `Bearer` prefix needed — the scheme is pre-configured as HTTP Bearer)
4. Click **Authorize** and then **Close**

All subsequent requests made through Swagger UI will include the `Authorization: Bearer <token>` header.

### How It Works

`AddSwaggerGenWithAuth()` (in `Extensions/SwaggerExtensions.cs`):
- Registers an **OpenAPI** document with version `v1`
- Adds a **Bearer** security definition (`SecuritySchemeType.Http` with scheme `bearer`)
- Enforces a **global security requirement** — every endpoint requires a Bearer token by default
- Individual endpoints can opt out using `[AllowAnonymous]` or `.AllowAnonymous()`

# Quickstart: API de Gestión de Faltas Tajamar

## Prerequisites

- .NET 8 SDK
- SQL Server instance reachable from the backend
- Access to the Tajamar external API base URL
- Environment variables or secret storage for external admin credentials and local JWT signing values

## Environment Variables

Set the following values before running the service:

- `ConnectionStrings__Default`
- `TajamarApi__BaseUrl`
- `TajamarApi__AdminUser`
- `TajamarApi__AdminPassword`
- `Jwt__Issuer`
- `Jwt__Audience`
- `Jwt__SigningKey`

## Local Setup Flow

1. Create or update the SQL Server database with the provided schema script.
2. Restore the backend solution once the codebase is present.
3. Start the API.
4. Authenticate a user against the local login endpoint.
5. Verify that the returned token exposes the expected role claims.
6. Exercise alumno, profesor, and administrador endpoints to confirm authorization boundaries.
7. Confirm that mirror synchronization can retrieve and refresh external user and course data with the admin service credentials.

## Examples

Login and get a token (replace credentials with a valid mirror user):

```bash
curl -X POST http://localhost:5209/api/auth/login \
	-H "Content-Type: application/json" \
	-d '{"email":"admin@example.com","password":"secret"}'
```

Use the token from the login response in the `Authorization` header:

```bash
curl -H "Authorization: Bearer <TOKEN>" http://localhost:5209/api/admin/faltas
```

Notes for Angular frontend (development):

- Ensure the Angular app runs on `http://localhost:4200` (default); the API enables a CORS policy named `AllowAngularDev` that permits this origin. If your frontend uses a different port, update `Program.cs` CORS settings accordingly.
- Use the local JWT returned by `/api/auth/login` as the Bearer token for authenticated requests from the frontend. Do not expose any admin external token to the client.
- Example Angular HTTP call:

```ts
// Example using HttpClient
const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
this.http.get<AdminFaltaDto[]>(`${apiBaseUrl}/api/admin/faltas`, { headers });
```

## Expected Validation Commands

- `dotnet restore`
- `dotnet test`
- `dotnet run --project backend/src/TajamarFaltas.Api`

## Operational Checks

- The external admin token should never be returned to clients.
- The local JWT should be the only token accepted by the API endpoints exposed to the Angular frontend.
- Missing or invalid external credentials should fail fast during synchronization and surface a server-side error.

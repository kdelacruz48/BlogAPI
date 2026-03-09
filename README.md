# BlogAPI

A containerized RESTful API built with ASP.NET Core and PostgreSQL, powering a personal blog platform. Handles authentication, role-based authorization, and full CRUD operations for blog posts and users.

Live at: `https://api.kylesisland.com`  
Frontend: [Kyle's Island](https://github.com/kdelacruz48/ReactFrontend)

---

## Architecture

The API follows the **Repository Pattern** to abstract data access from business logic, keeping controllers thin and the data layer swappable.

```
Request → Controller → IPostRepository → Entity Framework Core → PostgreSQL
```

**Auth flow:**
1. Client sends credentials to `/api/UserAuth/login`
2. API validates and returns a signed JWT
3. Client includes the token in the `Authorization: Bearer` header on subsequent requests
4. Role claims embedded in the token (`Admin` / `User` / `Guest`) control access at the endpoint level

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 Web API |
| Database | PostgreSQL |
| ORM | Entity Framework Core |
| Auth | JWT (JSON Web Tokens) |
| Containerization | Docker |
| Hosting | Railway |

---

## Endpoints

### Auth — `/api/UserAuth`

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/UserAuth/register` | None | Register a new user |
| POST | `/api/UserAuth/login` | None | Login and receive a JWT |

### Posts — `/api/BlogAPI`

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/BlogAPI` | Required | Get all posts (optional `?tag=` filter) |
| GET | `/api/BlogAPI/{id}` | Required | Get a single post by ID |
| POST | `/api/BlogAPI` | Admin | Create a new post |
| PUT | `/api/BlogAPI/{id}` | Admin | Update an existing post |
| DELETE | `/api/BlogAPI/{id}` | Admin | Delete a post |

**Roles:**
- `Admin` — full read/write access
- `User` — read access
- `Guest` — read-only, no registration required

---

## Data Models

### Post
```json
{
  "id": 1,
  "userName": "Kyle",
  "title": "Post title",
  "post": "Post content",
  "imageUrl": "https://example.com/image.jpg",
  "tag": "Tech",
  "created_date": "2025-01-15T00:00:00Z",
  "updated_date": "2025-03-08T00:00:00Z"
}
```

### API Response wrapper
All endpoints return a consistent response envelope:
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "errorMessages": [],
  "result": { }
}
```

---

## Local Development

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (optional, for containerized run)
- PostgreSQL instance (local or Railway)

### Running locally

1. Clone the repo
```bash
git clone https://github.com/kdelacruz48/BlogAPI
cd BlogAPI
```

2. Set environment variables (or add to `appsettings.Development.json`)
```bash
PrivateConnection=your_postgres_connection_string
ApiSettings__Secret=your_jwt_secret_key
```

3. Apply migrations and run
```bash
dotnet ef database update
dotnet run
```

4. Swagger UI available at `https://localhost:{port}/swagger`

### Running with Docker
```bash
docker build -t blogapi .
docker run -e PrivateConnection=your_conn_string -e ApiSettings__Secret=your_secret -p 8080:8080 blogapi
```

---

## Deployment (Railway)

1. Push to GitHub
2. Connect repo to a Railway project
3. Add environment variables in Railway dashboard:

| Variable | Description |
|---|---|
| `PrivateConnection` | PostgreSQL connection string |
| `ApiSettings__Secret` | JWT signing secret |

4. Railway auto-deploys on every push to `main`

---

## What's Next

- [ ] Rate limiting on public endpoints
- [ ] Structured logging with Serilog
- [ ] Image upload endpoint (currently posts reference external URLs)
- [ ] Unit tests with xUnit and Moq

---

## License

MIT — feel free to use and modify.

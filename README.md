# People Net Core Backend

A RESTful WebAPI built with ASP.NET Core 8.0 and Entity Framework Core that provides comprehensive CRUD operations for people management.

## Tech Stack

- **ASP.NET Core 8.0**
- **Entity Framework Core** (In-Memory provider)
- **xUnit** for unit testing
- **Swagger/OpenAPI** for API documentation
- **Docker** for containerization

## Features

- Complete CRUD operations for people management
- GET `/api/people` endpoint with pagination support
- GET `/api/people/{cpf}` endpoint to retrieve a specific person
- POST `/api/people` endpoint to create a new person
- PUT `/api/people/{cpf}` endpoint to update an existing person
- DELETE `/api/people/{cpf}` endpoint to remove a person
- Pagination with configurable page size (default: 10, max: 100)
- In-Memory database with seeded data
- Comprehensive unit tests
- Swagger documentation

## Person Model

Each person has the following properties:

| Property     | Type   | Description |
|--------------|--------|-------------|
| Cpf          | string | Unique identifier (CPF) |
| Name         | string | Person's full name |
| Genre        | string | Gender |
| Address      | string | Street address |
| Age          | int    | Person's age |
| Neighborhood | string | Neighborhood name |
| State        | string | State name |

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022, VS Code, or any compatible IDE
- Docker (optional, for containerized deployment)

### Running the Application

#### Option 1: Using .NET CLI

1. Clone the repository
2. Navigate to the project directory
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Run the application:
   ```bash
   dotnet run
   ```
5. Open your browser and navigate to:
   - API: `https://localhost:7000/api/people`
   - Swagger UI: `https://localhost:7000/swagger`

#### Option 2: Using Docker

1. Clone the repository
2. Navigate to the project directory
3. Build the Docker image:
   ```bash
   docker build -t people-net-core-backend .
   ```
4. Run the container:
   ```bash
   docker run -p 5000:5000 people-net-core-backend
   ```
5. Open your browser and navigate to:
   - API: `http://localhost:5000/api/people`
   - Swagger UI: `http://localhost:5000/swagger`

#### Docker Compose (Optional)

For easier development, you can use Docker Compose:

1. Create a `docker-compose.yml` file:
   ```yaml
   version: '3.8'
   services:
     people-api:
       build: .
       ports:
         - "5000:5000"
       environment:
         - ASPNETCORE_ENVIRONMENT=Development
   ```

2. Run with Docker Compose:
   ```bash
   docker-compose up --build
   ```

### Running Tests

To run the unit tests:

```bash
dotnet test
```

## API Endpoints

### GET /api/people

Returns a paginated list of people.

**Parameters:**
- `page` (optional): Page number (1-based, default: 1)
- `pageSize` (optional): Number of items per page (default: 10, max: 100)

**Response:**
```json
{
  "data": [
    {
      "cpf": "12345678901",
      "name": "João Silva",
      "genre": "Masculino",
      "address": "Rua das Flores, 123",
      "age": 30,
      "neighborhood": "Centro",
      "state": "São Paulo"
    },
    ...
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 30,
  "totalPages": 3,
  "hasPrevious": false,
  "hasNext": true
}
```

**Status Codes:**
- `200 OK`: People found and returned
- `400 Bad Request`: Invalid pagination parameters

### GET /api/people/{cpf}

Returns a specific person by CPF.

**Parameters:**
- `cpf` (string): The CPF of the person to retrieve

**Response:**
```json
{
  "cpf": "12345678901",
  "name": "João Silva",
  "genre": "Masculino",
  "address": "Rua das Flores, 123",
  "age": 30,
  "neighborhood": "Centro",
  "state": "São Paulo"
}
```

**Status Codes:**
- `200 OK`: Person found and returned
- `404 Not Found`: Person with specified CPF not found

### POST /api/people

Creates a new person.

**Request Body:**
```json
{
  "cpf": "99999999999",
  "name": "Maria Santos",
  "genre": "Feminino",
  "address": "Avenida Principal, 456",
  "age": 28,
  "neighborhood": "Vila Nova",
  "state": "Rio de Janeiro"
}
```

**Response:**
```json
{
  "cpf": "99999999999",
  "name": "Maria Santos",
  "genre": "Feminino",
  "address": "Avenida Principal, 456",
  "age": 28,
  "neighborhood": "Vila Nova",
  "state": "Rio de Janeiro"
}
```

**Status Codes:**
- `201 Created`: Person successfully created
- `400 Bad Request`: Invalid data or CPF already exists

### PUT /api/people/{cpf}

Updates an existing person.

**Parameters:**
- `cpf` (string): The CPF of the person to update

**Request Body:**
```json
{
  "cpf": "12345678901",
  "name": "João Silva Updated",
  "genre": "Masculino",
  "address": "Rua das Flores, 123",
  "age": 31,
  "neighborhood": "Centro",
  "state": "São Paulo"
}
```

**Response:**
```json
{
  "cpf": "12345678901",
  "name": "João Silva Updated",
  "genre": "Masculino",
  "address": "Rua das Flores, 123",
  "age": 31,
  "neighborhood": "Centro",
  "state": "São Paulo"
}
```

**Status Codes:**
- `200 OK`: Person successfully updated
- `400 Bad Request`: Invalid data or CPF mismatch
- `404 Not Found`: Person with specified CPF not found

### DELETE /api/people/{cpf}

Deletes a person by CPF.

**Parameters:**
- `cpf` (string): The CPF of the person to delete

**Response:** No content

**Status Codes:**
- `204 No Content`: Person successfully deleted
- `404 Not Found`: Person with specified CPF not found

## Project Structure

```
PeopleNetCoreBackend/
├── Controllers/
│   └── PeopleController.cs
├── Data/
│   └── PeopleDbContext.cs
├── Models/
│   └── Person.cs
├── PeopleNetCoreBackend.Tests/
│   ├── Controllers/
│   │   └── PeopleControllerTests.cs
│   ├── Data/
│   │   └── PeopleDbContextTests.cs
│   └── Models/
│       └── PersonTests.cs
├── Dockerfile
├── .dockerignore
├── PeopleNetCoreBackend.csproj
├── Program.cs
└── README.md
```

## Testing

The project includes comprehensive unit tests covering:

- **Controller Tests**: API endpoint functionality
- **Model Tests**: Person model validation
- **DbContext Tests**: Database operations and seeded data

Test coverage includes:
- Successful API responses
- JSON content validation
- Data integrity checks
- Unique constraint validation
- Model property validation

## Database

The application uses Entity Framework Core with an In-Memory database provider. The database is automatically seeded with 30 people when the application starts.

## Docker Configuration

The application includes a multi-stage Dockerfile optimized for production:

- **Build Stage**: Uses .NET 8.0 SDK to build and publish the application
- **Runtime Stage**: Uses .NET 8.0 runtime for a smaller production image
- **Security**: Runs as a non-root user for enhanced security
- **Ports**: Exposes ports 5000 and 5001
- **Environment**: Configured for production by default

### Docker Commands

```bash
# Build the image
docker build -t people-net-core-backend .

# Run the container
docker run -p 5000:5000 people-net-core-backend

# Run in development mode
docker run -p 5000:5000 -e ASPNETCORE_ENVIRONMENT=Development people-net-core-backend

# Run in detached mode
docker run -d -p 5000:5000 --name people-api people-net-core-backend

# View logs
docker logs people-api

# Stop the container
docker stop people-api

# Remove the container
docker rm people-api
```

## Development

To add new features or modify existing ones:

1. Make your changes
2. Run tests to ensure everything works:
   ```bash
   dotnet test
   ```
3. Run the application to test manually:
   ```bash
   dotnet run
   ```
4. Test with Docker (optional):
   ```bash
   docker build -t people-net-core-backend .
   docker run -p 5000:5000 people-net-core-backend
   ```

## License

This project is for educational purposes.

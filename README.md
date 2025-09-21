# People Net Core Backend

A RESTful WebAPI built with ASP.NET Core 7.0 and Entity Framework Core that exposes a GET endpoint to retrieve a list of people.

## Tech Stack

- **ASP.NET Core 7.0**
- **Entity Framework Core** (In-Memory provider)
- **xUnit** for unit testing
- **Swagger/OpenAPI** for API documentation

## Features

- Complete CRUD operations for people management
- GET `/api/people` endpoint that returns all people
- GET `/api/people/{cpf}` endpoint to retrieve a specific person
- POST `/api/people` endpoint to create a new person
- PUT `/api/people/{cpf}` endpoint to update an existing person
- DELETE `/api/people/{cpf}` endpoint to remove a person
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

- .NET 7.0 SDK or later
- Visual Studio 2022, VS Code, or any compatible IDE

### Running the Application

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

### Running Tests

To run the unit tests:

```bash
dotnet test
```

## API Endpoints

### GET /api/people

Returns a list of all people.

**Response:**
```json
[
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
]
```

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

## License

This project is for educational purposes.

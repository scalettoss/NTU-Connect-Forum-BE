# Forum Backend
## Prerequisites

Before running this application, make sure you have the following installed:

1. [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
2. [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
3. [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) (recommended) or [Visual Studio Code](https://code.visualstudio.com/)

## Setup Instructions

### 1. Clone the Repository

```bash
git clone [repository-url]
cd ForumBackEnd
```

### 2. Database Configuration

1. Open `appsettings.json` and `appsettings.Development.json`
2. Update the connection string to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ForumDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Apply Database Migrations

Using Package Manager Console in Visual Studio:
```bash
Update-Database
```

Or using .NET CLI:
```bash
dotnet ef database update
```

## Running the Application

### Using Visual Studio:

1. Open `ForumBackEnd.sln` in Visual Studio
2. Press F5 or click the "Run" button
3. The application will start and open in your default browser

### Using Command Line:

1. Navigate to the WebApplication1 directory:
```bash
cd WebApplication1
```

2. Run the application:
```bash
dotnet run
```

3. The API will be available at:
- HTTP: http://localhost:5244

## Project Structure

- `Controllers/`: API endpoints
- `Models/`: Database models
- `DTOs/`: Data Transfer Objects
- `Services/`: Business logic
- `Repositories/`: Data access layer
- `Migrations/`: Database migrations
- `Middlewares/`: Custom middleware components
- `SignalR/`: Real-time communication hubs

## API Documentation

Once the application is running, you can access the Swagger API documentation at:
- https://localhost:7223/swagger
- http://localhost:5223/swagger

## Troubleshooting

1. If you encounter database connection issues:
   - Verify SQL Server is running
   - Check connection string in appsettings.json
   - Ensure the database exists

2. If migrations fail:
   - Delete the Migrations folder
   - Create a new migration: `dotnet ef migrations add InitialCreate`
   - Update database: `dotnet ef database update`

3. Port conflicts:
   - Change ports in `Properties/launchSettings.json` if needed

## Support

For any issues or questions, please open an issue in the repository. 
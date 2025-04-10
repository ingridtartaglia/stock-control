# Stock Control System

A system for managing stock movements and generating stock reports.

## Features

- Stock movement registration (in/out)
- Stock report generation by date
- Product stock balance tracking
- Prevention of negative stock

## Technologies

### Backend
- .NET 9
- Entity Framework Core
- SQL Server
- Clean Architecture
- REST API

### Frontend
- React
- TypeScript
- Material UI
- Axios
- i18next

## Project Structure

```
├── API/                 # API layer
├── Application/         # Application layer
├── Domain/             # Domain layer
├── Infrastructure/     # Infrastructure layer
├── Test/               # Test layer
└── web/               # Frontend React application
```

## Getting Started

### Prerequisites
- .NET 9 SDK
- Node.js
- SQL Server (LocalDB)

### Backend Setup
1. Navigate to the `API` directory
2. Update the connection string in `appsettings.json` if needed
3. Run the following commands:
   ```bash
   dotnet restore
   dotnet run
   ```
   The API will be available at:
   - HTTP: http://localhost:5250
   - HTTPS: https://localhost:7074

### Frontend Setup
1. Navigate to the `web` directory
2. Run the following commands:
   ```bash
   npm install
   npm start
   ```
   The frontend will be available at:
   - http://localhost:3000

## Database

The system uses SQL Server with Entity Framework Core. The database will be automatically created and seeded with sample products when the application starts.

# GreenFlux

This project is an ASP.NET Core Web API designed for managing Groups, Charge Stations, and Connectors with robust features and modern development practices.

## Features

* Create, update, and remove Groups, Charge Stations, and Connectors.
* When a Group is removed, all associated Charge Stations are automatically removed.
* Charge Stations can only be added or removed from a Group one at a time.
* Each Charge Station belongs to exactly one Group; a Charge Station cannot exist without a Group.
* Connectors cannot exist without an associated Charge Station.
* The maximum current in Amps for a Connector can be updated.
* The Group's Capacity in Amps must always be greater than or equal to the sum of the maximum currents of all Connectors in the Group.

## Additional Features

* **Testing:** Implemented Unit Tests and Integration Tests.
* **Documentation:** Included Swagger for API documentation.
* **Database:** Uses SQL Server as the database.
* **Performance:** Optimized queries with EF Core Split Queries.
* **Architecture:** Implements the Repository pattern.
* **Error Handling:** Includes a global error handler.
* **Async Methods:** Utilized asynchronous methods for better performance.
* **Dockerization:** The application is fully dockerized with Docker Compose support.

## Technologies Used

* **IDE:** Visual Studio 2022
* **Framework:** .NET 8
* **Database:** SQL Server
* **Architecture:** Clean Architecture
* **Testing:** xUnit
* **Logging:** Serilog
* **ORM:** Entity Framework
* **Validation:** FluentValidation

## Setup Instructions

### Running Locally on Windows

#### Prerequisites

* Visual Studio 2022
* SQL Server installed locally

#### Steps

1. **Unzip the Project:** Extract the project files to a local directory.
2. **Update Connection String:**
   Add the following connection string to the `appsettings.json` file:

   ```json
   "DefaultConnection": "Server=.;Database=GreenFluxDb;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true"
   ```
3. **Run the API Project:**

   * Open the project in Visual Studio.
   * Start the API project. This will create the database and seed it with dummy data for testing.

### Running with Docker Compose

#### Prerequisites

* Docker Desktop for Windows

#### Steps

1. **Unzip the Project:** Extract the project files to a local directory.
2. **Update Connection String:**
   Add the following connection string to the `appsettings.json` file:

   ```json
   "DefaultConnection": "Server=sqlserver;Database=GreenFluxDb;User ID=sa;Password=P@55w0rd55;TrustServerCertificate=true;"
   ```
3. **Start Docker Compose:**

   * Right-click on the Docker Compose project in Visual Studio and select "Compose Up."
   * Alternatively, navigate to the project folder containing the Dockerfile, open a command window, and run:

     ```bash
     docker-compose up --build
     ```
   * This will build and run the application, including setting up SQL Server within a container.

## Testing the API

Once the application is running, you can access the API documentation via Swagger by navigating to:

```
http://localhost:8080/swagger/index.html
```

Use the provided Swagger UI to explore and test the API endpoints.

---

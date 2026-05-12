# Booking Events API

A robust, scalable backend API for managing event bookings, built with **.NET Core** and **C#**. This project follows **Clean Architecture** principles to separate business logic, infrastructure, and presentation, ensuring the codebase remains maintainable and testable.

## 🚀 Features

*   **Clean Architecture:** Separation of concerns using `API`, `Core`, and `Infrastructure` layers.
*   **Authentication & Authorization:** Secure user authentication using **JWT (JSON Web Tokens)**, including support for **Refresh Tokens** and **Role-Based Access Control** (e.g., Admin vs. Regular User).
*   **Event Management:** Full CRUD operations for events, including pagination and filtering by category.
*   **Booking System:** Users can book events and cancel their own bookings. Admins have overarching control.
*   **Standardized Responses:** All API endpoints return a consistent, generic `ApiResponse` wrapper to make frontend integration seamless.
*   **Entity Framework Core:** Uses EF Core as the ORM to interact with a SQL Server database.
*   **Swagger UI:** Built-in API documentation and testing interface.

## 🏗️ Architecture Breakdown

The solution is divided into three main projects:

1.  **`BookingEvents.Core`**: The heart of the application. Contains Entities, Data Transfer Objects (DTOs), Interfaces, and standard application settings. This layer has no dependencies on other layers or external frameworks.
2.  **`BookingEvents.Infrastructure`**: Contains the implementations of the interfaces defined in the Core layer. This includes Entity Framework Core Database Contexts, Repository implementations, and external service integrations (like Cloudinary for image hosting).
3.  **`BookingEvents.API`**: The presentation layer. Contains Controllers, ASP.NET Core configuration, Middleware (like global exception handling), and Dependency Injection setup.

## 🛠️ Technology Stack

*   **.NET 8.0** (or your targeted version)
*   **C#**
*   **Entity Framework Core (SQL Server)**
*   **ASP.NET Core Identity** (for User Management)
*   **JWT Bearer Authentication**
*   **Swagger / OpenAPI**

## 🚦 Getting Started

### Prerequisites

*   [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
*   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express / LocalDB).

### Setup Instructions

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/yourusername/BookingEvents.git
    cd BookingEvents
    ```

2.  **Update Database Connection String:**
    Open `BookingEvents.API/appsettings.json` (or `appsettings.Development.json`) and update the `DefaultConnection` string to point to your local SQL Server instance.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=BookingEventsDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

3.  **Configure JWT Settings:**
    Ensure your `appsettings.json` contains the necessary JWT settings:
    ```json
    "JWT": {
      "Key": "your_super_secret_key_here...",
      "Issuer": "your_issuer",
      "Audience": "your_audience",
      "DurationInMinutes": 60
    }
    ```

4.  **Apply Migrations:**
    Open a terminal in the root directory and apply the Entity Framework migrations to create the database:
    ```bash
    dotnet ef database update --project BookingEvents.Infrastructure --startup-project BookingEvents.API
    ```

5.  **Run the Application:**
    Navigate to the API directory and run the project:
    ```bash
    cd BookingEvents.API
    dotnet run
    ```

6.  **Access Swagger:**
    Once running, open your browser and navigate to `https://localhost:<port>/swagger` to explore and test the API endpoints.

## 🔐 API Endpoints Overview

*   **Auth:** `/api/Auth/register`, `/api/Auth/login`, `/api/Auth/refresh-token`, `/api/Auth/revoke-token`
*   **Events:** `/api/Events/events`, `/api/Events/event/{id}`, `/api/Events/category/{category}`, *(Admin)* `/api/Events/create-event`
*   **Bookings:** `/api/Bookings/book`, `/api/Bookings/user/{userId}`, `/api/Bookings/Cancel/{bookingId}`

## 🤝 Contributing

This project was built primarily for learning purposes. However, suggestions, issues, and pull requests are always welcome!

## 📝 License

This project is licensed under the MIT License.

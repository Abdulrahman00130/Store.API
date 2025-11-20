🛒 Store.API - E-Commerce Backend
-
Welcome to Store.API! This is a robust, scalable, and RESTful E-Commerce backend API built using ASP.NET Core 8.0.

The project follows the Onion Architecture principles to ensure separation of concerns and maintainability. It implements advanced patterns like the Specification Pattern, Unit of Work, and Repository Pattern, along with Redis for high-performance caching and Stripe for secure payment processing.

🔭 Project Overview
-
Store.API serves as the backbone for a modern online store. It manages the entire shopping lifecycle, from product catalog browsing with complex filtering to secure checkout and order history.

Core Capabilities:
-

Manages Products, Brands, and Categories.

Handles User Authentication and Authorization via Identity & JWT.

Provides a high-performance Shopping Basket using Redis.

Processes Orders and payments via Stripe Webhooks.

Implements server-side pagination, sorting, and searching.

🛠 Tech Stack
-
Frameworks & Languages
C# 12

.NET 8.0 (ASP.NET Core Web API)

Data & Persistence
Entity Framework Core 8 (Code-First approach)

SQL Server (Relational Database)

StackExchange.Redis (Distributed Caching & Basket storage)

Security
ASP.NET Core Identity (User Management)

JWT Bearer Authentication (Stateless Auth)

Third-Party Integrations
Stripe.net (Payment Gateway)

Utilities & Tools
AutoMapper (Object-Object Mapping)

Swagger / OpenAPI (API Documentation)

Dependency Injection (Built-in container)

✨ Key Features
-
🛍️ Product Catalog
Advanced Filtering: Filter by Brand, Type, and Search terms using the Specification Pattern.

Sorting & Pagination: Efficient data retrieval with PaginationResponse.

Caching: Custom [Cache] attribute to cache API responses in Redis for performance.

🛒 Shopping Basket
Redis Backed: Fast, temporary storage for customer baskets.

TTL: Baskets have a configurable time-to-live (e.g., 7 days).

🔐 Authentication & Identity
User Management: Registration, Login, and Profile management.

Address: Dedicated logic for managing user shipping addresses.

Role-Based Security: Support for Admin and User roles.

💳 Orders & Payments
Order Lifecycle: Create orders, track status (Pending, PaymentReceived, PaymentFailed).

Stripe Integration: Payment Intent creation and Webhook handling (PaymentsController) to verify successful transactions.

Delivery Methods: Configurable shipping options.

🛡️ Error Handling
Global Middleware: Centralized exception handling returning standardized ErrorDetails (RFC 7807 compliant style).

Validation: Fluent validation of DTOs (e.g., LoginRequest, OrderRequest).

📂 Folder Structure
-

```
Store.API
├── Core
│   ├── Store.API.Domain             # Entities, Enums, Repository Interfaces
│   ├── Store.API.Services.Abstractions # Service Interfaces
│   └── Store.API.Services           # Business Logic Implementation, Specifications
|
├── Infrastructure
│   ├── Store.API.Persistence        # EF Core, DbContext, Migrations, Redis, Repository, Unit Of Work 
│   └── Store.API.Presentation       # Controllers, Action Filters
|
├── Store.API.Shared                 # DTOs, Error Models, Constants
|
└── Store.API.Web                    # Program.cs, appsettings, DI Config
```

🚀 Getting Started
-
Prerequisites
.NET 8.0 SDK

SQL Server (LocalDB or Full)

Redis (Running locally or via Docker)

Stripe Account (For payment keys)

Installation
-
Clone the repository:
___________________________
git clone https://github.com/Abdulrahman00130/Store.API.git
___________________________

Configure AppSettings Update Store.API.Web/appsettings.json with your local configuration:
___________________________
```
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=Store.API.App;...",
  "IdentityConnection": "Server=.;Database=Store.API.Identity;...",
  "RedisConnection": "localhost"
},
"StripeOptions": {
  "Secretkey": "sk_test_..."
},
"JwtOptions": {
  "SecurityKey": "YourSuperSecretKey...",
  "Issuer": "https://localhost:7018",
  "Audience": "MyStore"
}
```
___________________________

Apply Migrations & Seed Data The project includes a DbInitializer. Simply run the application, and it will automatically apply pending migrations and seed initial data (Products, Brands, Types) from the JSON files in Data/DataSeeding.

🧪 Testing
-
You can test the API endpoints using Swagger UI (built-in) or Postman.

Authentication: Use the /api/Auth/register or /login endpoints to get a Token.

Payments: To test Stripe Webhooks locally, use the Stripe CLI to forward events to https://localhost:7018/api/Payments/webhook.

📸 Screenshots
-
Swagger Documentation
<img width="1915" height="898" alt="image" src="https://github.com/user-attachments/assets/a81ffc01-5fcf-4150-b048-bee761c98754" />
<img width="1920" height="897" alt="image" src="https://github.com/user-attachments/assets/df7e5def-4cd0-4e32-ae4d-90cffab8b3cf" />

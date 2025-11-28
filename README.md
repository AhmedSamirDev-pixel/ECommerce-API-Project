# E-Commerce API Project - Clean Architecture .NET 9  



A fully featured E-Commerce backend API built with .NET 9, applying Clean Architecture, Repository & Unit of Work, Redis caching, JWT authentication, and Stripe payments.
The system provides a scalable, modular, and production-ready backend for real-world E-Commerce applications.

---


## 🚀 Features 

### **🔐 Authentication & Authorization:** 

- JWT Authentication (Access + Refresh Tokens)

- Role-based Authorization (Admin / User)

- Email & Password login

- Secure password hashing & validation

### **🛒 Product & Category Management:** 

- Full CRUD for Products

- Image upload with local storage

- Pagination, Sorting, Filtering

- Product details, category-based listing

### **🧺 Shopping Cart (Basket):**

- Add / Remove / Update items

- Basket stored in Redis Cache

- Auto-binding basket to authenticated users

### **📦 Order Management:**

- Place orders from the basket

- Calculate total price, tax, shipping

- Track order status (Pending → Paid → Completed)

- View user order history

### **💳 Stripe Payment Integration:**

- Create payment intent

- Confirm payment

- Sync order status with Stripe webhook

- Secure handling of Stripe secret keys from environment variables

### 🛡️ **Error Handling:** 

- Implemented a centralized exception-handling middleware that provides consistent, structured, and unified API error responses across all endpoints.


### **⚡ Caching Layer:**

- Distributed caching using Redis

- Cached responses for products & categories

- Custom cache abstraction (ICacheRepo)

---


## 🧱 Project Architecture

The solution is structured using Clean Architecture, promoting separation of concerns, maintainability, and testability.

```

ECommerceSolution/
│
├── DomainLayer/
│   ├── ECommerce.Domain/                     # Core business rules
│   │   ├── Contracts/                        # Repository, Cache, UoW, Specs interfaces
│   │   ├── Exceptions/                       # Custom domain exceptions
│   │   ├── Models/                           # Entities (Products, Orders, Basket, Identity)
│   │   └── BaseEntity.cs                     # Shared base class
│   │
│   ├── ECommerce.Services/                   # Business logic layer
│   │   ├── BusinessServices/                 # Product, Basket, Order services
│   │   ├── MappingProfile/                   # AutoMapper profiles & resolvers
│   │   └── Specifications/                   # Specification pattern implementations
│   │
│   └── ECommerce.ServicesAbstraction/        # Service interfaces
│
├── InfrastructureLayer/
│   ├── ECommerce.Persistence/                # EF Core + Redis implementation
│   │   ├── Contexts/                         # DbContext & IdentityDbContext
│   │   ├── Configurations/                   # Fluent API configurations
│   │   ├── Repos/                            # Repository implementations
│   │   ├── Seed/                             # Data seeding
│   │   └── SpecificationsEvaluator.cs        # Specification translator
│   │
│   └── ECommerce.Presentation/               # API controllers & filters
│       ├── Controllers/                      # Product, Basket, Order, Auth controllers
│       └── Attribute/                        # Custom caching attribute
│
├── ECommerce.Shared/                         # DTOs & cross-cutting concerns
│   ├── Common/                               # Pagination, query params, sorting
│   └── DTOs/                                 # DTO models (Product, Basket, Order, Identity, Error)
│
└── ECommerce.Web/                            # API Host
    ├── CustomMiddlewares/                    # Global exception middleware
    ├── Program.cs                            # DI & pipeline setup
    └── wwwroot/Images/Products/              # Static product images

```

---


## 🛠️ Technologies & Design Patterns


| Category                | Technology / Pattern |
|-------------------------|--------------------|
| **Framework**           | .NET 9, ASP.NET Core |
| **Database**            | Entity Framework Core, SQL Server |
| **Caching**             | Redis (`StackExchange.Redis`) for basket management and response caching |
| **Authentication**      | ASP.NET Core Identity, JWT |
| **Payments**            | Stripe (`Stripe.net`) |
| **API Documentation**   | Swagger (OpenAPI) |
| **Object Mapping**      | AutoMapper |
| **Design Patterns**     | Clean Architecture <br> Repository & Unit of Work Pattern <br> Specification Pattern <br> Service Manager (Facade) |
 
---



## 📡 API Endpoints Overview



| Module             | Endpoint                                 | Method | Description                  |
| ------------------ | ---------------------------------------- | ------ | ---------------------------- |
| **Authentication** | `/api/Authentication/Login`              | POST   | Generates JWT token          |
|                    | `/api/Authentication/Register`           | POST   | Registers new user           |
|                    | `/api/Authentication/CurrentUser`        | GET    | Fetches authenticated user   |
|                    | `/api/Authentication/CurrentUserAddress` | GET    | Retrieves user address       |
|                    | `/api/Authentication/UpdateAddress`      | PUT    | Updates user address         |
| **Product**        | `/api/Product/products`                  | GET    | Paginated list with filters  |
|                    | `/api/Product/products/{id}`             | GET    | Product by ID                |
|                    | `/api/Product/brands`                    | GET    | All brands                   |
|                    | `/api/Product/types`                     | GET    | All types                    |
| **Basket**         | `/api/Basket?key={basketId}`             | GET    | Get basket                   |
|                    | `/api/Basket`                            | POST   | Create/update basket         |
|                    | `/api/Basket/{basketId}`                 | DELETE | Delete basket                |
| **Order**          | `/api/Order`                             | POST   | Create order                 |
|                    | `/api/Order/AllOrders`                   | GET    | User orders                  |
|                    | `/api/Order?orderId={guid}`              | GET    | Get order by ID              |
|                    | `/api/Order/DeliveryMethods`             | GET    | Delivery methods             |
| **Payment**        | `/api/Payment/{basketId}`                | POST   | Create/Update payment intent |

---


## ⚙️ Getting Started

Follow these instructions to get a copy of the project up and running on your local machine.

### **1. Clone Repository:**

```
git clone https://github.com/ahmedsamirdev-pixel/ECommerce-API-Project.git
cd ECommerce-API-Project
```


### **2. Update appsettings.json**

Inside ECommerce.Web/appsettings.json:

```
"ConnectionStrings": {
  "DefaultConnection": "Data Source=localhost;Initial Catalog=ECommerceDb;Integrated Security=True;Trust Server Certificate=True",
  "RedisConnection": "localhost",
  "IdentityConnection": "Data Source=localhost;Initial Catalog=ECommerceIdentityDb;Integrated Security=True;Trust Server Certificate=True"
}
```


### **3. Configure Stripe Secret Key:**

Set environment variable:  

Variable: STRIPE_SECRET_KEY

Value: sk_test_...

Example (Windows):

```
setx STRIPE_SECRET_KEY "sk_test_123456"
```

 
### **4. Run the Application:**

```
cd ECommerce.Web
dotnet run
```


### **5. Explore the API:**

Swagger is available at:

```
https://localhost:7251/swagger
```

---



## 📌 Future Enhancements

- Product Reviews & Ratings

- Email notifications

- Admin Dashboard

- Distributed caching enhancements

- Background jobs (Hangfire)

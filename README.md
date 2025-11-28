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

##  📡 Sample API Requests
*Postman Collection Link:**  
[Ahmed Samir E-Commerce API Collection](https://ahmedsemido14-7056598.postman.co/workspace/Ahmed-Samir's-Workspace~23f67931-289d-48f9-8322-0b8577b13b53/collection/49307448-1bbf5056-93ef-4c62-a3ba-eaf069ace0c9?action=share&source=copy-link&creator=49307448)

### Sample API Requests

1️⃣ Register a New User

```

POST /api/Authentication/Register
Content-Type: application/json

{
    "Email": "mahmoud@gmail.com",
    "DisplayName": "Mahmoud Hesham",
    "UserName": "mahmoudhesham",
    "Password": "Ali@Yaseer0",
    "PhoneNumber":"02992884342"
}

```

2️⃣ Login


```
POST /api/Authentication/Login
Content-Type: application/json

{
    "Email": "Mahmoud@gmail.com",
    "Password": "Ali@Yaseer0"
}
```

Response Example:

```
{
    "email": "mahmoud@gmail.com",
    "displayName": "Mahmoud Hesham",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJtYWhtb3VkQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJtYWhtb3VkaGVzaGFtIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJjODRmMWQ3NC02MzA4LTQ5MzctOGI0MC0wZGMzZDg3YzU3MDEiLCJleHAiOjE3NjQ0ODEyMDcsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNTEvIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI1MS9hcGkifQ.JrvEljWrQXjFaT3K7LnnCFpBxtRAPkBCH9C9LC6kMmY"
}
```


3️⃣ Get Products List


```
GET /api/Product/products?pageIndex=1&pageSize=5
Authorization: Bearer <JWT_TOKEN>
```

Response Example:

```
{
    "pageIndex": 1,
    "pageSize": 5,
    "totalCount": 18,
    "data": [
        {
            "id": 1,
            "name": "Italian Chicken Marinade",
            "description": "This Italian dressing chicken marinade is a super simple but delicious way to add flavor before grilling.",
            "pictureUrl": "https://localhost:7251/images/products/ItalianChickenMarinade.png",
            "price": 200.000,
            "brandName": "Chicken",
            "typeName": "Italian"
        },
        {
            "id": 2,
            "name": "Chicken Marsala",
            "description": "Chicken Marsala is an Italian-style recipe for tender pan-fried chicken breasts in a sweet Marsala wine and mushroom sauce. It's super quick and easy to make for a weeknight dinner AND sophisticated enough for company.",
            "pictureUrl": "https://localhost:7251/images/products/ChickenMarsala.png",
            "price": 150.000,
            "brandName": "Chicken",
            "typeName": "Italian"
        },
        {
            "id": 3,
            "name": "Hearty Vegetable Lasagna",
            "description": "This hearty, vegetable lasagna is the only lasagna my husband will eat. We love it! Hope you all enjoy it as much as we do.",
            "pictureUrl": "https://localhost:7251/images/products/HeartyVegetableLasagna.png",
            "price": 180.000,
            "brandName": "Seafood",
            "typeName": "Italian"
        },
        {
            "id": 4,
            "name": "Cheesy Vegetable Lasagna",
            "description": "A rich, cheesy lasagna loaded with vegetables. You could also omit all veggies except broccoli for a broccoli lasagna.",
            "pictureUrl": "https://localhost:7251/images/products/CheesyVegetableLasagna.png",
            "price": 300.000,
            "brandName": "Seafood",
            "typeName": "Italian"
        },
        {
            "id": 5,
            "name": "Pasta Salad",
            "description": "This pasta salad recipe was given to me by a dear friend many years ago, and I've been making it ever since! It's great for barbecues.",
            "pictureUrl": "https://localhost:7251/images/products/PastaSalad.png",
            "price": 250.000,
            "brandName": "Soup",
            "typeName": "Italian"
        }
    ]
}
```


4️⃣ Get Product Details by ID


```
GET /api/Product/products/12
Authorization: Bearer <JWT_TOKEN>
```

Response Example:


```
{
    "id": 12,
    "name": "Haluski - Cabbage and Noodles",
    "description": "This haluski recipe is made with buttery egg noodles, fried cabbage, and onions. It's a great, flavorful dish!",
    "pictureUrl": "https://localhost:7251/images/products/HaluskiNoodles.png",
    "price": 16.000,
    "brandName": "Soup",
    "typeName": "Chinese"
}
```
    
5️⃣ Add Or Update Basket


```
POST /api/Basket
Content-Type: application/json
Authorization: Bearer <JWT_TOKEN>
```

```
{
  "id": "basket03",
  "items": [
    {
      "id": 7,
      "name": "External Hard Drive 1TB",
      "pictureUrl": "https://example.com/images/external-hard-drive.jpg",
      "price": 59.99,
      "quantity": 1
    },
    {
      "id": 8,
      "name": "Gaming Headset",
      "pictureUrl": "https://example.com/images/gaming-headset.jpg",
      "price": 89.99,
      "quantity": 1
    },
    {
      "id": 9,
      "name": "Monitor Stand Riser",
      "pictureUrl": "https://example.com/images/monitor-stand.jpg",
      "price": 29.99,
      "quantity": 1
    }
  ],
  "DeliveryMethodId": 3
}

```

Response Example:


```
{
    "id": "basket03",
    "items": [
        {
            "id": 7,
            "name": "External Hard Drive 1TB",
            "pictureUrl": "https://example.com/images/external-hard-drive.jpg",
            "price": 59.99,
            "quantity": 1
        },
        {
            "id": 8,
            "name": "Gaming Headset",
            "pictureUrl": "https://example.com/images/gaming-headset.jpg",
            "price": 89.99,
            "quantity": 1
        },
        {
            "id": 9,
            "name": "Monitor Stand Riser",
            "pictureUrl": "https://example.com/images/monitor-stand.jpg",
            "price": 29.99,
            "quantity": 1
        }
    ],
    "clientSecret": null,
    "paymentIntentId": null,
    "deliveryMethodId": 3,
    "shippingPrice": null
}
```

6️⃣ Create/Update Payment Intent


```
POST /api/Payment/basket03
Authorization: Bearer <JWT_TOKEN>
```

Response Example:


```
{
    "id": "basket03",
    "items": [
        {
            "id": 7,
            "name": "External Hard Drive 1TB",
            "pictureUrl": "https://example.com/images/external-hard-drive.jpg",
            "price": 10.000,
            "quantity": 1
        },
        {
            "id": 8,
            "name": "Gaming Headset",
            "pictureUrl": "https://example.com/images/gaming-headset.jpg",
            "price": 8.000,
            "quantity": 1
        },
        {
            "id": 9,
            "name": "Monitor Stand Riser",
            "pictureUrl": "https://example.com/images/monitor-stand.jpg",
            "price": 15.000,
            "quantity": 1
        }
    ],
    "clientSecret": "pi_3SYKe1AjVOptwnqM1nPYg3iC_secret_pRR0aic14gciMvuUOaFx2OzOd",
    "paymentIntentId": "pi_3SYKe1AjVOptwnqM1nPYg3iC",
    "deliveryMethodId": 3,
    "shippingPrice": 2.00
}
```

7️⃣ Create Order


```
POST /api/Order
Content-Type: application/json
Authorization: Bearer <JWT_TOKEN>

{
  "basketId": "basket03",
  "deliveryMethodId": 2,
  "address": {
    "street": "123 Nile St",
    "city": "Cairo",
    "country": "Egypt",
    "firstName": "Mahmoud",
    "lastName": "Zaied"
  }
}
```

Response Example:


```
{
    "id": "46d189d4-bced-4af0-d2a2-08de2e42f52a",
    "userEmail": "hager.fathy@gmail.com",
    "orderDate": "2025-11-28T07:57:04.8867709+02:00",
    "address": {
        "street": "123 Nile St",
        "city": "Cairo",
        "country": "Egypt",
        "firstName": "Mahmoud",
        "lastName": "Zaied"
    },
    "deliveryMethod": "UPS2",
    "orderStatus": "Pending",
    "items": [
        {
            "productName": "External Hard Drive 1TB",
            "pictureUrl": "https://localhost:7251/https://example.com/images/external-hard-drive.jpg",
            "price": 10.000,
            "quantity": 1
        },
        {
            "productName": "Gaming Headset",
            "pictureUrl": "https://localhost:7251/https://example.com/images/gaming-headset.jpg",
            "price": 8.000,
            "quantity": 1
        },
        {
            "productName": "Monitor Stand Riser",
            "pictureUrl": "https://localhost:7251/https://example.com/images/monitor-stand.jpg",
            "price": 15.000,
            "quantity": 1
        }
    ],
    "subtotal": 33.000,
    "total": 38.000
}
```


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


### **3. Database Migrations & Seeding:**

This project uses **Entity Framework Core** migrations to manage database schemas and seed initial data.

#### **1. Main Application Database (`StoreDbContext`)**

Run the following command to apply migrations:

```bash
cd ECommerce.Persistence
dotnet ef database update --context StoreDbContext
```

#### **Included Migrations:**

    - 20251117204247_AddProductModule → Product module setup
    
    - 20251117211637_Rename PictureUrl → PictureUrl fix
    
    - 20251125043959_AddOrderModule → Order module setup
    
    - 20251127211445_AddPaymentIntentIdColumnToBasket → Add PaymentIntentId to orders

The StoreDbContextModelSnapshot.cs is kept up to date.


#### **2. Identity Database (StoreIdentityDbContext)**

Run the following command to apply Identity migrations:

```
cd ECommerce.Persistence
dotnet ef database update --context StoreIdentityDbContext
```

#### **Included Migrations:**

    - 20251124042329_IdentityModule → Identity module with custom ApplicationUser
    
    - The StoreIdentityDbContextModelSnapshot.cs is kept up to date.
    

⚠️ On first run, the application automatically applies any pending migrations and seeds the database with initial data (Products, Categories, Users, Roles).




### **4. Configure Stripe Secret Key:**

Set environment variable:  

Variable: STRIPE_SECRET_KEY

Value: sk_test_...

Example (Windows):

```
setx STRIPE_SECRET_KEY "sk_test_123456"
```

 
### **5. Run the Application:**

```
cd ECommerce.Web
dotnet run
```


### **6. Explore the API:**

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

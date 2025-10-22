# E-Commerce Web API Project Plan

## 1. Overview
This project aims to build a full-featured e-commerce backend using **ASP.NET Core Web API** and **Entity Framework Core**. It will support user management, product catalog, cart, orders, payments, inventory, and reviews.

---

## 2. Core Features
- **User Management**: Registration, login, JWT authentication, roles (Admin/Customer)
- **Product Management**: CRUD operations, categories, subcategories
- **Cart System**: Add/remove items, update quantity, checkout
- **Order Management**: Place orders, order history, order status updates
- **Payment Handling**: Integrate with multiple payment methods
- **Inventory Management**: Track stock levels, prevent overselling
- **Reviews and Ratings**: Customer product feedback

---

## 3. Database Design

### 3.1 Entity Relationships Diagram
```
Users ─┬──< Orders ─┬──< OrderItems >──┬── Products >── Categories
        │             │
        │             └── Payment
        │
        └──< Carts ───< CartItems
                       
Products ───< Reviews >── Users
Products ───< Inventory
```

### 3.2 Tables

#### **Users**
| Column | Type | Description |
|--------|------|-------------|
| UserId | INT (PK) | Unique ID |
| Username | NVARCHAR(50) | Unique username |
| Email | NVARCHAR(100) | Unique email |
| PasswordHash | NVARCHAR(MAX) | Hashed password |
| Role | NVARCHAR(20) | ('Admin', 'Customer') |
| CreatedAt | DATETIME | Registration date |
| UpdatedAt | DATETIME | Last update |

#### **Products**
| Column | Type | Description |
|--------|------|-------------|
| ProductId | INT (PK) | Unique ID |
| Name | NVARCHAR(100) | Product name |
| Description | NVARCHAR(MAX) | Details |
| Price | DECIMAL(10,2) | Product price |
| CategoryId | INT (FK) | Linked to Category |
| ImageUrl | NVARCHAR(255) | Optional |
| IsActive | BIT | Visible for sale |
| CreatedAt | DATETIME | Added date |

#### **Categories**
| Column | Type | Description |
|--------|------|-------------|
| CategoryId | INT (PK) | Unique ID |
| Name | NVARCHAR(100) | Category name |
| Description | NVARCHAR(255) | Optional |
| ParentCategoryId | INT (nullable FK) | For subcategories |

#### **Cart**
| Column | Type | Description |
|--------|------|-------------|
| CartId | INT (PK) | Unique ID |
| UserId | INT (FK) | Owner |
| CreatedAt | DATETIME | Date created |

#### **CartItems**
| Column | Type | Description |
|--------|------|-------------|
| CartItemId | INT (PK) | Unique ID |
| CartId | INT (FK) | Linked to Cart |
| ProductId | INT (FK) | Linked to Product |
| Quantity | INT | Item quantity |

#### **Orders**
| Column | Type | Description |
|--------|------|-------------|
| OrderId | INT (PK) | Unique ID |
| UserId | INT (FK) | Linked to user |
| OrderDate | DATETIME | When placed |
| Status | NVARCHAR(30) | ('Pending', 'Paid', 'Shipped', 'Delivered', 'Cancelled') |
| TotalAmount | DECIMAL(10,2) | Total amount |
| ShippingAddress | NVARCHAR(255) | Delivery location |

#### **OrderItems**
| Column | Type | Description |
|--------|------|-------------|
| OrderItemId | INT (PK) | Unique ID |
| OrderId | INT (FK) | Linked to order |
| ProductId | INT (FK) | Linked to product |
| Quantity | INT | Number of units |
| UnitPrice | DECIMAL(10,2) | Price at purchase |

#### **Payments**
| Column | Type | Description |
|--------|------|-------------|
| PaymentId | INT (PK) | Unique ID |
| OrderId | INT (FK) | Linked to Order |
| PaymentMethod | NVARCHAR(50) | ('CreditCard', 'UPI', 'COD', etc.) |
| PaymentStatus | NVARCHAR(30) | ('Pending', 'Completed', 'Failed') |
| TransactionId | NVARCHAR(100) | Payment gateway ID |
| PaymentDate | DATETIME | Date processed |

#### **Inventory**
| Column | Type | Description |
|--------|------|-------------|
| InventoryId | INT (PK) | Unique ID |
| ProductId | INT (FK) | Linked to Product |
| StockQuantity | INT | Current stock |
| LastUpdated | DATETIME | Updated timestamp |

#### **Reviews**
| Column | Type | Description |
|--------|------|-------------|
| ReviewId | INT (PK) | Unique ID |
| ProductId | INT (FK) | Linked to Product |
| UserId | INT (FK) | Linked to User |
| Rating | INT | 1–5 stars |
| Comment | NVARCHAR(255) | Review text |
| CreatedAt | DATETIME | Submitted date |

---

## 4. Implementation Steps

### **Phase 1: Project Setup**
- Create ASP.NET Core Web API project
- Setup Entity Framework Core with SQL Server
- Configure database connection
- Create migration and seed data

### **Phase 2: User Management**
- Implement registration and login endpoints
- Add JWT authentication and role-based authorization

### **Phase 3: Product and Category Management**
- CRUD for products and categories (Admin)
- Public endpoints to view products and categories

### **Phase 4: Cart and Orders**
- Create cart endpoints (add/remove/update items)
- Implement order placement logic
- Add order history and order status APIs

### **Phase 5: Payment and Inventory**
- Mock payment gateway integration
- Deduct inventory stock post-purchase
- Manage stock updates

### **Phase 6: Reviews**
- Add product reviews and ratings endpoints
- Fetch average rating per product

### **Phase 7: Testing & Deployment**
- Use Swagger for API testing
- Implement unit tests with xUnit or NUnit
- Deploy using Azure or AWS

---

## 5. Optional Future Enhancements
- Wishlist system
- Coupons and discounts
- Admin dashboard for analytics
- Email notifications
- Advanced search and filtering

---

## 6. Tech Stack
- **Backend**: ASP.NET Core 8 Web API
- **Database**: SQL Server / SQLite (for dev)
- **ORM**: Entity Framework Core
- **Authentication**: JWT / ASP.NET Identity
- **Testing**: xUnit
- **Hosting**: Azure / AWS EC2 / Render

---

## 7. Outcome
A robust, modular, and scalable e-commerce API that demonstrates mastery of:
- RESTful API design
- Database relationships and EF Core migrations
- Secure user authentication and role management
- Clean architecture principles


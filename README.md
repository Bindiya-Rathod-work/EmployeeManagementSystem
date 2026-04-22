# Employee Management System

A role-based Employee Management System built with **ASP.NET Core Razor Pages**, **Entity Framework Core**, and **PostgreSQL**. 

---

## 🧰 Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 .0 Razor Pages |
| Database | PostgreSQL 18 (via Npgsql) |
| ORM | Entity Framework Core 10 (Code-First) |
| Authentication | ASP.NET Core Identity |
| UI | Bootstrap 5, Bootstrap Icons, DataTables.js |
| Alerts | SweetAlert2 |

---

## 🏗️ Project Architecture

The project follows a clean **layered architecture** with clear separation of concerns:
EmployeeManagementSystem
│
├── Models/                  → Database entity classes (Employee, EmployeeDocument)
├── ViewModels/              → UI-specific models (never expose entities to views)
├── Data/                    → DbContext and SeedData
├── Repositories/
│   ├── Interfaces/          → IEmployeeRepository, IDocumentRepository
│   └── Implementations/     → EmployeeRepository, DocumentRepository
├── Services/
│   ├── Interfaces/          → IEmployeeService, IDocumentService
│   └── Implementations/     → EmployeeService, DocumentService
├── Pages/
│   ├── Admin/               → Dashboard, Employees, AddEmployee, EditEmployee,
│   │                          EmployeeDetails, ViewDocument
│   ├── EmployeePortal/      → Profile
│   └── Auth/                → Login, Logout, AccessDenied
└── Uploads/                 → PDF files stored here (excluded from Git)

---

## ✨ Features

### Admin
- Secure login with role-based access
- Dashboard with live statistics (total employees, joined this month, total documents)
- Add new employees with auto-generated login credentials
- View, edit, and soft delete employee records
- Upload, view (inline modal), and delete PDF documents per employee
- DataTables-powered employee list with search and pagination

### Employee
- Secure login
- View own basic profile information
- Access denied with proper redirect for unauthorized pages

### General
- Soft delete — records are never permanently removed, preserving audit trail
- Global query filters in EF Core automatically exclude deleted records
- Role-based authorization using ASP.NET Core Identity
- Admins excluded from employee list using Identity role lookup

---

## ⚙️ Setup Instructions

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 18](https://www.postgresql.org/download/)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) with ASP.NET workload

### 1. Clone the Repository
```bash
git clone https://github.com/Bindiya-Rathod-work/EmployeeManagementSystem.git
cd EmployeeManagementSystem
```

### 2. Configure Database Connection
Open `appsettings.json` and update the connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=EmployeeManagementDB;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

### 3. Apply Migrations
Open **Package Manager Console** in Visual Studio and run:
Update-Database

This will create the database and all tables automatically.

### 4. Run the Application
Press **F5** in Visual Studio or run:
```bash
dotnet run
```

The application will seed a default Admin account on first run.

---

## 🔐 Default Credentials

| Role | Email | Password |
|---|---|---|
| Admin | admin@ems.com | Admin@1234 |
| Employee | *(created by Admin)* | *(set by Admin)* |

---

## 📁 Key Design Decisions

- **Soft Delete** — Employees and documents are never hard deleted. `IsDeleted` flag is used with EF Core global query filters so deleted records are invisible to all queries automatically.
- **Repository Pattern** — Abstracts data access logic away from business logic, making the codebase testable and maintainable.
- **ViewModel Separation** — Entity models are never passed directly to Razor Pages. Dedicated ViewModels prevent over-posting attacks and keep UI concerns separate.
- **Identity Integration** — `Employee` extends `IdentityUser` so authentication, password hashing, and role management are handled securely by ASP.NET Core Identity.
- **Admin Role Filter** — Admins are excluded from employee list by querying the Identity role table directly, not by hardcoding emails.

---

## 👨‍💻 Author

Developed by Bindiya

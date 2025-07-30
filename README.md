# ClinicCore Management 🏥


## 🎯 Overview

A comprehensive Clinic Management System built with ASP.NET Core 9 Web API, designed specifically for specialized medical clinics.
Built as a learning project to demonstrate proficiency in modern .NET technologies, it implements industry best practices for healthcare data management with a focus on security, scalability, and maintainability.

### Problem Statement
Medical clinics often struggle with fragmented systems for patient management, appointment scheduling, and medical record keeping. ClinicCore Management addresses these challenges by providing a unified, secure platform that streamlines all clinic operations.

### Target Audience
- **Super Administrators**: Complete system oversight and management
- **Clinic Managers**: Operational management and reporting
- **Receptionists**: Patient registration and appointment scheduling
- **Medical Administrators**: Medical record and prescription management

## ✨ Key Features

### 👥 Patient Management
- **Complete CRUD Operations**: Create, read, update, and delete patient records
- **Advanced Search**: Search patients by name, phone, email, or medical record number
- **Patient History**: Comprehensive medical history tracking
- **Data Validation**: Robust validation for all patient data inputs

### 📅 Appointment Management
- **Flexible Scheduling**: Book, reschedule, and cancel appointments
- **Doctor Assignment**: Assign specific doctors to appointments
- **Status Tracking**: Track appointment status (scheduled, completed, cancelled)
- **Appointment Completion**: Complete appointments with integrated medical records and payments

### 👨‍⚕️ Doctor Management
- **Doctor Profiles**: Maintain detailed doctor information and specializations
- **Schedule Management**: Manage doctor availability and schedules
- **Performance Tracking**: Track doctor appointments and patient interactions

### 📋 Medical Records
- **Comprehensive Records**: Detailed medical history and diagnosis tracking
- **Linked Appointments**: Medical records automatically linked to appointments
- **Audit Trail**: Complete history of medical record changes

### 💊 Prescription Management
- **Digital Prescriptions**: Create and manage digital prescriptions
- **Medication Tracking**: Track prescribed medications, dosages, and frequencies
- **Prescription History**: Complete prescription history per patient
- **Validation Rules**: Prevent duplicate prescriptions for same medical record

### 💳 Payment Management
- **Payment Processing**: Record and track all clinic payments
- **Payment History**: Complete payment audit trail
- **Integration**: Seamlessly integrated with appointment completion workflow

### 🔐 Advanced Security
- **JWT Authentication**: Secure token-based authentication with refresh tokens
- **Role-Based Authorization**: Granular permission control with claims-based policies
- **Multi-layered Security**: Comprehensive security model with role-claim hierarchy
- **Token Management**: Secure token generation, refresh, and revocation

## 🏗️ Architecture

The ClinicCore Management follows a clean architecture pattern organized into four distinct layers implemented as separate .NET projects.
Each layer has specific responsibilities and maintains clear dependency boundaries to ensure separation of concerns and maintainability.

```
┌─────────────────┐
│   API Layer     │ ← Controllers, Middleware, Authentication
├─────────────────┤
│ Business Layer  │ ← Services, Validation, Business Logic
├─────────────────┤
│Data Access Layer│ ← Repositories, EF Core, Database Context
├─────────────────┤
│  Domain Layer   │ ← Entities, DTOs, Constants, Interfaces
└─────────────────┘
```

### Architectural Patterns
- **Repository Pattern** – Introduces an abstraction layer over data access logic, enabling decoupling between the business logic and data source implementation. This improves testability and adherence to the Single Responsibility Principle.
- **Unit of Work Pattern** – Coordinates the writing of changes across multiple repositories as a single transaction, ensuring data consistency and integrity within business operations.
- **Dependency Injection** – Promotes loose coupling by injecting services and dependencies at runtime, enhancing modularity, testability, and alignment with the Inversion of Control (IoC) principle.


## 🔐 Security & Authentication

### Authentication Flow
1. **User Registration**: Create account with role assignment
2. **Login**: Authenticate with email/password
3. **JWT Generation**: Receive access token and refresh token
4. **API Access**: Use Bearer token for authenticated requests
5. **Token Refresh**: Refresh expired tokens seamlessly
6. **Token Revocation**: Secure logout and token invalidation

<img width="1611" height="782" alt="Image" src="https://github.com/user-attachments/assets/b2c3d04a-be5e-464e-af64-9a53d0e0ccd1" />

### Authorization Model
The system implements a hierarchical **Role-Claim** authorization model:

```
User → Roles → Claims → Permissions
```

### Security Features
- **Password Hashing**: Secure password storage with ASP.NET Core Identity
- **Token Expiration**: Configurable JWT and refresh token lifespans
- **Claim-Based Policies**: Granular permission control
- **HTTPS Enforcement**: Secure communication protocol
- **Request Validation**: Comprehensive input validation and sanitization
  
### Default System Roles
The system automatically seeds four predefined roles:
- **SuperAdmin**: Complete system access
- **ClinicManager**: Operational management capabilities
- **Receptionist**: Patient and appointment management
- **MedicalAdmin**: Medical records and prescription management

  ## 🧪 Development Features

### Request/Response Logging
The system includes comprehensive request/response logging middleware that captures:
- HTTP method and endpoints
- Request/response headers
- Request/response body (with sensitive data filtering)
- Response times and status codes
- Correlation IDs for request tracking

### Global Error Handling
Centralized error handling middleware that:
- Catches unhandled exceptions
- Provides consistent error responses
- Logs errors with correlation context
- Returns appropriate HTTP status codes

### Data Validation
Robust validation system using FluentValidation:
- Model validation for all DTOs
- Custom business rule validation
- Cross-field validation rules
- Localized error messages
  

  ## 📚 API Documentation

### Authentication Endpoints
```http
POST   /api/Auth/register          # User registration
POST   /api/Auth/login             # User authentication
POST   /api/Auth/refresh-token     # Refresh JWT token
POST   /api/Auth/revoke-token      # Revoke refresh token
```

### Patient Management
```http
GET    /api/Patient                # Get all patients
GET    /api/Patient/{id}           # Get patient by ID
POST   /api/Patient                # Create new patient
PUT    /api/Patient/{id}           # Update patient
DELETE /api/Patient/{id}           # Delete patient
```

### Appointment Management
```http
GET    /api/Appointment            # Get all appointments
GET    /api/Appointment/{id}       # Get appointment by ID
POST   /api/Appointment            # Create appointment
PUT    /api/Appointment/{id}       # Update appointment
DELETE /api/Appointment/{id}       # Cancel appointment
POST   /api/Appointment/{id}/complete # Complete appointment
PUT    /api/Appointment/{id}/reschedule # Reschedule appointment
```

### Doctor Management
```http
GET    /api/Doctor                 # Get all doctors
GET    /api/Doctor/{id}            # Get doctor by ID
POST   /api/Doctor                 # Add new doctor
PUT    /api/Doctor/{id}            # Update doctor
DELETE /api/Doctor/{id}            # Delete doctor
```

### Medical Records
```http
GET    /api/MedicalRecord          # Get all medical records
GET    /api/MedicalRecord/{id}     # Get medical record by ID
POST   /api/MedicalRecord          # Create medical record
PUT    /api/MedicalRecord/{id}     # Update medical record
DELETE /api/MedicalRecord/{id}     # Delete medical record
```

### Prescription Management
```http
GET    /api/Prescription           # Get all prescriptions
GET    /api/Prescription/{id}      # Get prescription by ID
POST   /api/Prescription           # Create prescription
PUT    /api/Prescription/{id}      # Update prescription
DELETE /api/Prescription/{id}      # Delete prescription
```

### Payment Management
```http
GET    /api/Payment                # Get all payments
GET    /api/Payment/{id}           # Get payment by ID
POST   /api/Payment                # Record payment
PUT    /api/Payment/{id}           # Update payment
```

### Role & Permission Management
```http
GET    /api/Role                   # Get all roles
POST   /api/Role                   # Create role
PUT    /api/Role/{id}              # Update role
DELETE /api/Role/{id}              # Delete role
POST   /api/UserRole/add-user-to-role    # Assign user to role
POST   /api/UserRole/remove-user-from-role # Remove user from role
GET    /api/RoleClaim              # Get all role claims
POST   /api/RoleClaim              # Create role claim
PUT    /api/RoleClaim/{id}         # Update role claim
DELETE /api/RoleClaim/{id}         # Delete role claim
```

## 📁 Project Structure

```
ClinicCore Management/
├── ClinicAPI/                    # API Layer
│   ├── Controllers/              # API Controllers
│   ├── Extensions/               # Service extensions
│   ├── Middlewares/              # Custom middleware
│   └── Program.cs                # Application entry point
├── BusinessLayer/                # Business Logic Layer
│   ├── Mapping/                  # AutoMapper profiles
│   ├── Services/                 # Business services
│   └── Validations/              # FluentValidation rules
├── DataAccessLayer/              # Data Access Layer
│   ├── Migrations/               # EF Core migrations
│   ├── Persistence/              # Database context
│   ├── Repositories/             # Repository implementations
│   ├── Seeding/                  # Data seeding
│   └── UnitOfWork/               # Unit of Work pattern
└── DomainLayer/                  # Domain Layer
    ├── Constants/                # Application constants
    ├── DTOs/                     # Data Transfer Objects
    ├── Enums/                    # Enumeration types
    ├── Helpers/                  # Utility classes
    ├── Interfaces/               # Service interfaces
    └── Models/                   # Entity models
```


## 🛠️ Technology Stack

### Backend Framework
- **.NET 9.0**: Latest .NET framework for high performance
- **ASP.NET Core 9.0**: Web API framework for RESTful services

  ### Database & ORM
- **SQL Server** – A reliable and high-performance relational database management system used to store and manage structured application data with strong support for indexing, transactions, and security.
- **Entity Framework Core 9.0** – A modern, lightweight, and extensible ORM that enables developers to interact with the database using strongly-typed C# models through the Code First approach.
- **Database Migrations** – Allows seamless and version-controlled evolution of the database schema directly from the codebase, ensuring consistency across development, staging, and production environments.


  ### Authentication & Security
- **ASP.NET Core Identity** – Provides robust user management, password hashing, and authentication workflows out of the box, fully integrated with the system’s role and claim infrastructure.
- **JWT Bearer Tokens** – Implements stateless, secure token-based authentication for scalable APIs, with custom claims embedded for fine-grained access control.
- **Refresh Tokens** – Enables secure and persistent session management by allowing token renewal without re-authentication, with full rotation and expiration strategies.
- **Claims-Based Authorization** – Facilitates granular and flexible access control by mapping permissions to roles and evaluating claims at the policy level.


### Development Tools
- **AutoMapper** – Enables clean and maintainable object-to-object mapping between domain models and DTOs, minimizing boilerplate code and keeping controller logic concise.
- **FluentValidation** – Provides a fluent, expressive, and extensible way to validate incoming models, enforcing business rules with clean separation from controller logic.
- **Swagger / OpenAPI** – Delivers comprehensive, interactive API documentation for developers, with full request/response models, XML comments integration, and out-of-the-box testing capabilities.
- **Serilog** – A powerful structured logging library that supports multiple output sinks (database, console, Seq, etc.), enabling real-time diagnostics and performance monitoring across environments.


### Additional Libraries
- **Microsoft.AspNetCore.Authentication.JwtBearer**: JWT authentication
- **Microsoft.EntityFrameworkCore.SqlServer**: SQL Server provider
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore**: Identity integration
- **Swashbuckle.AspNetCore**: Swagger integration

## 🗄️ Database Schema

The system uses a comprehensive database schema with 17 tables:

### Core Entities
- **Patients**: Patient information and demographics
- **Doctors**: Doctor profiles and specializations
- **Appointments**: Appointment scheduling and management
- **MedicalRecords**: Medical history and diagnoses
- **Prescriptions**: Medication prescriptions and details
- **Payments**: Payment records and transaction history

### Identity & Security
- **AspNetUsers**: User accounts and authentication
- **AspNetRoles**: System roles (SuperAdmin, ClinicManager, etc.)
- **AspNetUserRoles**: User-role associations
- **AspNetRoleClaims**: Role-based permissions
- **AspNetUserClaims**: User-specific claims
- **AspNetUserTokens**: JWT and refresh tokens
- **RefreshTokens**: Refresh token management

### System Tables
- **Logs**: Application logging and audit trail
- **__EFMigrationsHistory**: Database migration history
- **sysdiagrams**: Database schema diagrams
  
<img width="2236" height="4496" alt="Image" src="https://github.com/user-attachments/assets/2be74d42-5b9e-4c40-bd83-772b11aee809" />

### 🚀 Getting Started with Admin Access

To quickly explore and test the system, you can use the pre-seeded **Admin User** account. This account is already assigned the `SuperAdmin` role, which has full access to all secured endpoints and system features.

**Login Endpoint:**
```
  POST /api/auth/login
```
**Sample Admin Credentials:**
```json
{
  "email": "admin@example.com",
  "password": "StrongPassword123###"
}
```
After a successful login, you will receive a JWT Bearer Token and a Refresh Token. Use the JWT token in the Authorization header for all authenticated requests


## 🚀 Getting Started

### Prerequisites
- **.NET 9.0 SDK** or later
- **SQL Server 2019** or later (LocalDB supported)
- **Visual Studio 2022** or **Visual Studio Code**
- **SQL Server Management Studio** (optional)

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Zyad-Eltayabi/ClinicCore-Management.git
   cd ClinicCore-Management
   ```

2. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

3. **Configure Database Connection**
   
   Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
    "default": "Data Source=.;Database=Clinic;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;"
     }
   }
   ```

4. **Apply Database Migrations**
   ```bash
   dotnet ef database update --project DataAccessLayer --startup-project ClinicAPI
   ```
   ##### or using Package Manager Console
   ```
     Update-Database
   ```

6. **Run the Application**
   ```bash
   dotnet run --project ClinicAPI
   ```

7. **Access Swagger Documentation**
   
   Navigate to: `https://localhost:7238/swagger` or `http://localhost:5202/swagger`


### Coding Standards
- Follow C# coding conventions
- Document public APIs with XML comments
- Use meaningful commit messages


**Built with ❤️ using ASP.NET Core 9**

⭐ **Star this repository if you love it!**


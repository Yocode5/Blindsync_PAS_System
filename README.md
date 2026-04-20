# Blind-Match Project Approval System (PAS)

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](#)
[![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-blue)](#)
[![Tests](https://img.shields.io/badge/tests-18%20passed-success)](#)

The **Blind-Match Project Approval System** is a secure, enterprise-grade web application built to facilitate anonymous project proposal matching between university students and academic supervisors. Engineered with a strict focus on data integrity, Role-Based Access Control (RBAC), and automated Quality Assurance.

---

## Key Features

### Security & Architecture
* **Role-Based Access Control (RBAC):** Secure routing and authorization for Admin, Supervisor, and Student workflows.
* **Session Lifecycle Management:** Hardened session destruction with strict HTTP `cache-control` headers (`no-store`) to prevent browser navigation exploits.
* **ACID Transactions:** Complex operations, such as user provisioning with specific role quotas, are handled within single database transactions to guarantee safe rollbacks.
* **Soft Delete Architecture:** Destructive hard-deletions are replaced with an `IsActive` toggling system to permanently preserve relational data integrity and audit trails.

### Admin User Management
* **Aggregated ViewModels:** Strict separation of concerns between database entities and the presentation layer.
* **Dynamic Modals:** Highly reusable frontend UI components that dynamically render fields based on the selected user role.

### Blind-Matching Workflow
* **Anonymous Review:** Student identities are hidden during the initial proposal review phase to eliminate bias.
* **State Management:** Explicitly tracks proposal lifecycles through `Pending`, `Under Review`, and `Matched` states.

---

## Technology Stack

* **Backend Framework:** ASP.NET Core MVC
* **ORM & Database:** Entity Framework (EF) Core, MS SQL Server
* **Frontend Design:** HTML5, CSS3, JavaScript (Prototyped in Figma)
* **Testing:** xUnit, Moq, WebApplicationFactory
* **CI/CD Pipeline:** GitHub Actions

---

## Quality Assurance & CI/CD

This application is protected by a multi-tiered testing suite acting as an automated Quality Gate, guaranteeing that no broken code reaches the main branch. 

* **Unit Testing:** Pure business logic is tested in absolute isolation using the `Moq` framework to simulate data streams.
* **Functional & Integration Testing:** End-to-end routing, middleware authorization, and database state management are tested using `WebApplicationFactory`.
* **Ephemeral Testing Environments:** The startup sequence is engineered to dynamically swap the production SQL Server for an In-Memory Database during testing, ensuring cross-platform execution and lightning-fast pipeline speeds.

---

## Getting Started

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
* SQL Server (Express or Developer Edition)
* Visual Studio 2022 / JetBrains Rider

### Local Setup
1. **Clone the repository**
   ```bash
   git clone https://github.com/Yocode5/Blindsync_PAS_System.git

---

## Default Test Credentials

Use the following credentials to explore the different Role-Based workflows:

**Admin** 
Email: `admin@uni.ac.lk` 
Password: `admin123`

**Student** 
Email: `student@uni.ac.lk` 
Password: `student123` 

**Supervisor**
Email: `supervisor@uni.ac.lk` 
Password: `super123`

**⚠️ Important Note:** Before running the application, you must update the connection string in `appsettings.Development.json` to match your local SQL Server instance so the system can successfully connect to the database.



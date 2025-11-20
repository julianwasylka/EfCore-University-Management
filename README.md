# University Management System (EF Core & Clean Architecture)

## 📝 Overview
This project is a comprehensive implementation of a University Management System designed to demonstrate advanced **Entity Framework Core** capabilities within a **Clean Architecture** structure.

The solution focuses on **Domain-Driven Design (DDD)** principles, enforcing business rules (such as custom index numbering) within the application layer using transactions, and optimizing database performance through efficient LINQ queries.

## 🏗 Architecture
The solution follows a strict separation of concerns:

1.  **Domain** (Class Library):
    * Contains enterprise logic and entities (`Student`, `Professor`, `Course`).
    * Implements **Value Objects** (`Address`) using C# Records.
    * Defines Domain models for inheritance (`MasterStudent`) and relations.
2.  **Application** (Class Library):
    * Handles business logic and CRUD operations.
    * **Key Feature:** Implements a transactional "Sequence Generator" for academic indexes (e.g., `S1001`, `P2002`) using a dedicated `SequenceCounter` entity.
    * Ensures gap-free numbering logic upon deletion (decrementing counters if the last entity is removed).
3.  **Infrastructure** (Class Library):
    * EF Core configuration using **Fluent API**.
    * Database Context (`UniversityDbContext`) and Migrations.
    * Advanced mappings: Owned Types, Many-to-Many with payload (`Enrollment`), Self-Referencing (`Prerequisites`).
    * **Data Seeding:** Integrated **Bogus** library to generate realistic data respecting business logic.
4.  **UI** (Console Application):
    * Minimalistic CLI for interacting with the system.
    * Reporting module for executing complex queries.

## ✨ Key Features
* **Transactional Numbering System:** Custom logic to generate prefixed IDs (e.g., 'S' for Student) stored in a separate sequence table, fully ACID-compliant.
* **Advanced EF Modeling:**
    * **Owned Entity Types** (Address).
    * **Table-Per-Hierarchy (TPH)** Inheritance.
    * **Many-to-Many** relationships with intermediate attributes.
    * **Self-Referencing** Many-to-Many relationships (Course Prerequisites).
* **Performance Optimization:**
    * Use of `AsNoTracking` for read-only operations.
    * Prevention of N+1 problems using `Include/ThenInclude`.
    * Server-side evaluation enforcement.
    * Data Projections (`Select`) to minimize data transfer.

## 🛠 Tech Stack
* **Language:** C# (.NET 8)
* **ORM:** Entity Framework Core
* **Database:** MS SQL Server (LocalDB)
* **Libraries:**
    * `Bogus` (Data generation)
    * `Microsoft.EntityFrameworkCore`

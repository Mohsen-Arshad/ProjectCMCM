# ProjectCMCM

A document-based request management API built with **ASP.NET Core Web API**.

This was my **first programming project**, where I started learning C#, .NET, databases, REST APIs, authentication, and backend development from scratch.

## About

ProjectCMCM allows users to create and manage requests and attach supporting documents to them.

The application uses **SQL Server** for application data and **Azure Blob Storage** for uploaded files.

### Features

* User registration and login
* JWT authentication
* Request management
* Category management
* Document upload and download
* Azure Blob Storage integration
* SQL Server database
* Swagger / OpenAPI

## Tech Stack

* **C# / .NET 7**
* **ASP.NET Core Web API**
* **Dapper**
* **SQL Server**
* **Azure Blob Storage**
* **JWT**
* **Swagger**

## Project Structure

```text
CMCMApp/
├── ApiCMCM/
│   ├── Controllers/
│   ├── StartupConfig/
│   └── Program.cs
│
└── LibraryCMCM/
    ├── Models/
    ├── DataAccess/
    └── CustomValidationAttribute/
```

## Getting Started

```bash
git clone https://github.com/Mohsen-Arshad/ProjectCMCM.git
cd ProjectCMCM
dotnet restore
dotnet build
dotnet run --project CMCMApp/ApiCMCM
```

The application requires a configured **SQL Server database** and **Azure Blob Storage**.

---

### A Starting Point

This project represents where my journey in programming began.

It was built before I had experience with AI-assisted development, and it remains in my GitHub as a record of that starting point and how my development skills have evolved since then.

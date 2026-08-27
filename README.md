# <div dir="rtl" align="center">JobBoardPlatform</div>

<kbd>
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 9" />
  <img src="https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?style=for-the-badge&logo=aspnetcore&logoColor=white" alt="MVC" />
  <img src="https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server" />
  <img src="https://img.shields.io/badge/Redis-Cache-DC382D?style=for-the-badge&logo=redis&logoColor=white" alt="Redis" />
  <img src="https://img.shields.io/badge/Swagger-API-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger" />
</kbd>

> A complete job board platform that connects **employers** and **job seekers** — from posting advertisements and building resumes to tracking applications and closing positions once the hiring is done.

---

## ✨ Features

### 👨‍💼 For Job Seekers

- Register / sign in with **email or phone number**
- Build a rich profile: resume, education, experience, skills & photo
- Search & filter advertisements by *category, city, job type, salary range, seniority*
- Apply to positions & track the application lifecycle: `Pending → Reviewing → Interview → Accepted / Rejected`
- Get notified whenever your application status changes

### 🏢 For Employers

- Register and manage a **company profile**
- Post, edit and remove **job advertisements**
- **Close advertisements** once the position is filled — new applications stop, history stays intact
- Review applications and change their status with validation
- Promote ads with **featured packages** configured by the admin

### 🛡️ For Admin

- Manage job categories, jobs, cities, provinces & skills
- **Approve / reject employer registrations**
- Configure featured pricing packages
- Dashboard with usage statistics

---

## 🧱 Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core (.NET 9) — MVC + Web API |
| ORM | Entity Framework Core 9 (Code-First + Migrations) |
| Queries (API) | Dapper |
| Database | SQL Server |
| Caching | Redis |
| Job Scheduling | Hangfire |
| Auth | JWT (Access + Refresh tokens) + Identity Roles/Claims |
| Emailing | MailKit (SMTP / Gmail) |
| Logging | Serilog |
| Frontend | Bootstrap 5, jQuery, Razor Views (dark/light mode) |

---

## 📁 Solution Structure

```
JobBoardPlatform.slnx
├── JobBoardPlatform.Core                  # Entities · enums · constants
├── JobBoardPlatform.Application           # Services · DTOs · validation
├── JobBoardPlatform.Infrastructure        # DbContext · migrations · repositories
├── JobBoardPlatform.Infrastructure.Dapper # Dapper read queries
├── JobBoardPlatform.Mvc                   # Web app (employer / seeker / admin)
└── JobBoardPlatform.WebApi                # REST API (job search)
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (any edition)
- Redis *(optional — app works without it)*

### 1️⃣ Database

Set the connection string in `JobBoardPlatform.Mvc/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=JobBoardPlatformDB;User Id=sa;Password=yourpassword;TrustServerCertificate=True"
}
```

### 2️⃣ Migrations

```bash
dotnet ef database update --project JobBoardPlatform.Infrastructure --startup-project JobBoardPlatform.Mvc
```

### 3️⃣ Secrets (not committed)

```bash
dotnet user-secrets init --project JobBoardPlatform.Mvc
```

| Secret | Purpose |
|---|---|
| `JwtSettings:Secret` | JWT signing key (long random string) |
| `JwtSettings:EncryptKey` | JWT encryption key (16 chars) |
| `AdminData:Password` | Initial admin account password |
| `SmtpSettings:Password` | Gmail app password |
| `ConnectionStrings:DefaultConnection` | *(optional)* connection string |

### 4️⃣ Run

```bash
dotnet run --project JobBoardPlatform.Mvc
```

| App | URL |
|---|---|
| 🌐 Web app | http://localhost:5080 |
| 🔌 API (Swagger) | http://localhost:5000/swagger |

### 5️⃣ Admin account

Seeded automatically on first launch:

| | |
|---|---|
| **Email** | `HemenKarimi8424@Gmail.com` |
| **Phone** | `09114945251` |
| **Password** | value of `AdminData:Password` |

> New employers register through the app — the admin approves them from the admin panel.

---

## 📝 Notes

- A fresh database is created by applying migrations; only **roles + admin account** are seeded automatically.
- Logs are written to `Logs/log-*.txt` and the `Logs` table in SQL Server.
- Background jobs (email notifications, feature expiry) run via Hangfire at `/hangfire`.

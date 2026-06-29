# PayFlow — Payroll Management System


## What is this?

PayFlow is a full-stack payroll management system I built for a real client — the principal of a playschool. She needed something to replace the manual salary tracking she was doing, so I built this from scratch.

It has two sides — an admin portal for the principal to manage everything, and an employee portal where teachers can check their own payslips and attendance. The whole thing runs on a REST API so it can later be connected to a mobile app without changing the backend at all.

---

## Features

### Admin Side
- Add, edit, activate/deactivate employees
- Upload employee photos
- Create login accounts for employees with a temporary password
- Reset employee passwords
- Enter monthly attendance for all employees at once
  - Set working days globally (auto-fills for all)
  - Add paid leaves per employee
  - See real-time deduction preview before saving
- Generate monthly payroll (auto-calculates deduction from unpaid leaves)
- Add incentives/bonuses
- Edit deductions and recalculate with latest salary
- Mark payroll as paid (locks permanently after that)
- View all payslips with filter by month/status
- Monthly reports with charts
- Branch settings — school name, logo, contact info

### Employee Side
- Forced password change on first login
- View own profile, update contact info
- Upload own profile photo
- View salary breakdown with charts
- Browse payslip history
- View attendance history

### How deductions work
```
Effective Working Days = Working Days - Paid Leaves
Unpaid Leaves          = Effective Working Days - Days Present
Per Day Salary         = Basic Pay ÷ Effective Working Days
Deduction              = Per Day Salary × Unpaid Leaves
Net Pay                = Basic Pay + Allowance + Incentive - Deduction
```
Paid leaves don't affect salary. Only unpaid leaves get deducted.

---

## Tech Stack

**Backend**
- ASP.NET Core 10 Web API
- Clean Architecture (Domain / Application / Infrastructure / API)
- Entity Framework Core + PostgreSQL
- JWT Authentication
- BCrypt password hashing

**Frontend**
- HTML, CSS, JavaScript (no framework)
- Bootstrap 5
- Chart.js for graphs
- Font Awesome icons

---

## Project Structure

```
PayFlow/
├── PayFlow.Domain/          → Entities, exceptions, enums
├── PayFlow.Application/     → Interfaces, DTOs, business contracts  
├── PayFlow.Infrastructure/  → Repositories, services, EF Core, JWT
└── PayFlow.API/             → Controllers, middleware, Program.cs

PayFlow.Frontend/
├── index.html               → Login page
├── change-password.html
├── admin/                   → All admin pages
├── employee/                → All employee pages
└── assets/                  → CSS, JS, images
```

---

## Screenshots

### Login & Auth
| Login | Change Password |
|---|---|
| ![](screenshots/login.jpg) | ![](screenshots/changepassword.png) |

### Admin
| Dashboard | Employees |
|---|---|
| ![](screenshots/Admin-Dashboard.png) | ![](screenshots/Admin-Employee.png) | 

| Add Employee | Employee Detail |
|---|---|
| ![](screenshots/Admin-Employeeaddemployee.png) | ![](screenshots/Admin-Employeedetailview.png) |

| Attendance | Payroll |
|---|---|
| ![](screenshots/Admin-Attendance.png) | ![](screenshots/Admin-Payroll.png) |

| Payroll Detail | Payslips |
|---|---|
| ![](screenshots/Admin-Payrolldetailview.png) | ![](screenshots/Admin-Payslips.png) |

| Reports | Settings |
|---|---|
| ![](screenshots/Admin-Reports.png) | ![](screenshots/Admin-Settings.png) |

### Employee Portal
| Dashboard | Profile |
|---|---|
| ![](screenshots/Employee-Dashboard.png) | ![](screenshots/Employee-Profile.png) |

| Payslips | Attendance |
|---|---|
| ![](screenshots/Employee-Payslips.png) | ![](screenshots/Employee-Attendance.png) |

---

## Running Locally

**Requirements**
- .NET 10 SDK
- PostgreSQL
- VS Code with Live Server extension

**Backend**
```bash
git clone https://github.com/YOUR_USERNAME/PayFlow.git
cd PayFlow
dotnet restore

# Update appsettings.json with your DB details
dotnet run --project PayFlow.API
# API runs at http://localhost:5091
# Swagger at http://localhost:5091/swagger
```

**Database**
```sql
-- Create database
CREATE DATABASE PayFlowDb;

-- Then run payflow_schema.sql in pgAdmin
```

**Frontend**
```bash
cd PayFlow.Frontend
# Open in VS Code
# Right click index.html → Open with Live Server
# Opens at http://127.0.0.1:5500
```

**Default login**
```
Username: admin
Password: Admin@123
```

---

## Deployment

Hosted for free using:

| Service | What it does |
|---|---|
| Supabase | PostgreSQL database |
| Render.com | ASP.NET Core API |
| Netlify | Frontend |

🔗 Live: *coming soon*

---

## What I learned building this

This was my first real client project — not a college assignment, an actual working system someone uses. I did the full thing: requirement gathering, database design, backend API, frontend, and deployment.

Some specific things:
- Clean Architecture in practice (not just theory)
- JWT auth with role-based middleware
- Why salary snapshots matter (if you live-join salary, a hike breaks historical payslips)
- File uploads in ASP.NET Core
- How to think about edge cases before they become bugs

---

## What's next

- Flutter mobile app (the API is already ready for it)
- PDF payslip download
- Email notifications
- Multi-branch support

---

## About

Built by **Yashvi Nikam** — CS graduate, currently interning as a Junior Software Engineer.

[![GitHub](https://img.shields.io/badge/GitHub-Yashvi--Nikam-black?style=flat&logo=github)](https://github.com/Yashvi-Nikam)

---

## 📄 License

This project is developed for a real client and portfolio purposes.

---

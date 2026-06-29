-- ============================================================
--  PayFlow — School Payroll Management System
--  Database Schema (PostgreSQL) — FINAL UPDATED VERSION
--  Last updated: June 2026
-- ============================================================

-- ============================================================
--  EXTENSIONS
-- ============================================================
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- ============================================================
--  TABLE: Employee
-- ============================================================
CREATE TABLE "Employee" (
    "EmployeeId"            SERIAL          PRIMARY KEY,
    "Name"                  VARCHAR(100)    NOT NULL,
    "Designation"           VARCHAR(100)    NOT NULL,
    "Address"               TEXT,
    "Contact"               VARCHAR(15),
    "Email"                 VARCHAR(150)    UNIQUE,
    "Aadhaar"               VARCHAR(12)     UNIQUE,
    "DateOfJoining"         DATE            NOT NULL,
    "BasicPay"              NUMERIC(10, 2)  NOT NULL DEFAULT 0,
    "ConveyanceAllowance"   NUMERIC(10, 2)  NOT NULL DEFAULT 0,
    "PhotoPath"             VARCHAR(300),
    "IsActive"              BOOLEAN         NOT NULL DEFAULT TRUE,
    "CreatedAt"             TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    "UpdatedAt"             TIMESTAMPTZ     NOT NULL DEFAULT NOW()
);

-- ============================================================
--  TABLE: Users (note: renamed from User)
-- ============================================================
CREATE TABLE "Users" (
    "UserId"            SERIAL          PRIMARY KEY,
    "Username"          VARCHAR(100)    NOT NULL UNIQUE,
    "PasswordHash"      TEXT            NOT NULL,
    "Role"              VARCHAR(20)     NOT NULL
                            CHECK ("Role" IN ('Admin', 'Employee')),
    "EmployeeId"        INT             UNIQUE
                            REFERENCES "Employee"("EmployeeId")
                            ON DELETE SET NULL,
    "IsFirstLogin"      BOOLEAN         NOT NULL DEFAULT TRUE,
    "LastLoginAt"       TIMESTAMPTZ,
    "CreatedAt"         TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    "UpdatedAt"         TIMESTAMPTZ     NOT NULL DEFAULT NOW()
);

-- ============================================================
--  TABLE: Attendance
--  PaidLeaves added — unpaid leaves auto calculated
-- ============================================================
CREATE TABLE "Attendance" (
    "AttendanceId"  SERIAL  PRIMARY KEY,
    "EmployeeId"    INT     NOT NULL
                        REFERENCES "Employee"("EmployeeId")
                        ON DELETE RESTRICT,
    "Month"         INT     NOT NULL
                        CHECK ("Month" BETWEEN 1 AND 12),
    "Year"          INT     NOT NULL
                        CHECK ("Year" >= 2000),
    "WorkingDays"   INT     NOT NULL
                        CHECK ("WorkingDays" > 0),
    "DaysPresent"   INT     NOT NULL
                        CHECK ("DaysPresent" >= 0),
    "PaidLeaves"    INT     NOT NULL DEFAULT 0,
    "CreatedAt"     TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "UpdatedAt"     TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT "CHK_Attendance_DaysPresent"
        CHECK ("DaysPresent" <= "WorkingDays"),

    CONSTRAINT "UQ_Attendance_Employee_Month_Year"
        UNIQUE ("EmployeeId", "Month", "Year")
);

-- ============================================================
--  TABLE: Payroll
--  Incentive added — for bonuses
-- ============================================================
CREATE TABLE "Payroll" (
    "PayrollId"     SERIAL          PRIMARY KEY,
    "EmployeeId"    INT             NOT NULL
                        REFERENCES "Employee"("EmployeeId")
                        ON DELETE RESTRICT,
    "Month"         INT             NOT NULL
                        CHECK ("Month" BETWEEN 1 AND 12),
    "Year"          INT             NOT NULL
                        CHECK ("Year" >= 2000),
    "BasicPay"      NUMERIC(10, 2)  NOT NULL DEFAULT 0,
    "Allowance"     NUMERIC(10, 2)  NOT NULL DEFAULT 0,
    "Incentive"     NUMERIC(10, 2)  NOT NULL DEFAULT 0,
    "Deduction"     NUMERIC(10, 2)  NOT NULL DEFAULT 0,
    "NetPay"        NUMERIC(10, 2)  NOT NULL DEFAULT 0,
    "IsPaid"        BOOLEAN         NOT NULL DEFAULT FALSE,
    "PaidDate"      DATE,
    "CreatedAt"     TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    "UpdatedAt"     TIMESTAMPTZ     NOT NULL DEFAULT NOW(),

    CONSTRAINT "CHK_Payroll_NetPay"
        CHECK ("NetPay" >= 0),

    CONSTRAINT "UQ_Payroll_Employee_Month_Year"
        UNIQUE ("EmployeeId", "Month", "Year")
);

-- ============================================================
--  TABLE: BranchSettings
--  School name, logo, contact info
-- ============================================================
CREATE TABLE "BranchSettings" (
    "Id"            SERIAL PRIMARY KEY,
    "SchoolName"    VARCHAR(200) NOT NULL,
    "PrincipalName" VARCHAR(100),
    "Email"         VARCHAR(150),
    "Address"       TEXT,
    "Contact"       VARCHAR(15),
    "LogoPath"      VARCHAR(300),
    "CreatedAt"     TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "UpdatedAt"     TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- ============================================================
--  INDEXES
-- ============================================================
CREATE INDEX "IDX_Employee_IsActive"        ON "Employee"("IsActive");
CREATE INDEX "IDX_Attendance_Month_Year"    ON "Attendance"("Month", "Year");
CREATE INDEX "IDX_Payroll_IsPaid"           ON "Payroll"("IsPaid");
CREATE INDEX "IDX_Payroll_Month_Year"       ON "Payroll"("Month", "Year");

-- ============================================================
--  SEED DATA
--  Run this AFTER creating tables.
--  Generate real BCrypt hash using the temp endpoint then update.
-- ============================================================
INSERT INTO "Users" (
    "Username", "PasswordHash", "Role", "EmployeeId", "IsFirstLogin"
) VALUES (
    'admin',
    '$2a$12$REPLACETHISWITHAREALBCRYPTHASHOFYOURCHOSENPASSWORD......',
    'Admin',
    NULL,
    FALSE
);

INSERT INTO "BranchSettings" ("SchoolName", "PrincipalName")
VALUES ('Playgroup Branch', 'Principal');

-- ============================================================
--  VIEWS
-- ============================================================
CREATE OR REPLACE VIEW "vw_Attendance" AS
SELECT
    "AttendanceId",
    "EmployeeId",
    "Month",
    "Year",
    "WorkingDays",
    "DaysPresent",
    "PaidLeaves",
    ("WorkingDays" - "PaidLeaves" - "DaysPresent") AS "UnpaidLeaves",
    "CreatedAt",
    "UpdatedAt"
FROM "Attendance";

CREATE OR REPLACE VIEW "vw_PayrollDetail" AS
SELECT
    p."PayrollId",
    e."EmployeeId",
    e."Name"        AS "EmployeeName",
    e."Designation",
    p."Month",
    p."Year",
    p."BasicPay",
    p."Allowance",
    p."Incentive",
    p."Deduction",
    p."NetPay",
    p."IsPaid",
    p."PaidDate"
FROM "Payroll" p
JOIN "Employee" e ON e."EmployeeId" = p."EmployeeId";

-- ============================================================
--  END OF SCHEMA
-- ============================================================

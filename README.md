# Health Care System API

DEPI Round 5 — **Session 01** (foundation)

ASP.NET Core Web API on **.NET 10** with **EF Core**, **SQL Server** and **Swagger**.

---

## Session 01 contents

### Entities — `Health Care System/Entities`

| Entity | Fields |
|---|---|
| `BaseEntity` | `Id`, `IsDeleted`, `CreatedAt`, `CreatedBy`, `ModifiedAt`, `ModifiedBy` |
| `Patient` | `Name`, `DateOfBirth`, `Appointments` |
| `Doctor` | `Name`, `Specialization`, `Appointments` |
| `Appointment` | `PatientId`, `DoctorId`, `AppointmentDate` |

### Database model — `Health Care System/Data/ApplicationDbContext.cs`

- `Patient` → `Appointments` (one-to-many)
- `Doctor` → `Appointments` (one-to-many)
- `Patient` ↔ `Doctor` (many-to-many through `Appointment`)
- Composite primary key: `(PatientId, DoctorId, AppointmentDate)`
- `DeleteBehavior.Restrict` on both foreign keys

### Interceptors — `Health Care System/Data/Interceptors`

| Interceptor | Behaviour |
|---|---|
| `CreatedAtInterceptor` | fills `CreatedAt` and `ModifiedAt` on insert |
| `ModifiedAtInterceptor` | refreshes `ModifiedAt` on update |
| `SoftDeleteInterceptor` | turns a delete into `IsDeleted = true` |

### Migration — `Health Care System/Data/Migrations`

`InitialCreate` creates the tables `Patients`, `Doctors` and `Appointments`.

### API setup

- Swagger UI with a `Bearer` security scheme (`Extensions/ApplicationServiceExtension.cs`)
- SQL Server connection string in `appsettings.Development.json`
- Interceptors registered on the `DbContext`

---

## Run locally

```powershell
dotnet ef database update --project "Health Care System\Health Care System.csproj"
dotnet run --project "Health Care System\Health Care System.csproj"
```

Swagger UI: `http://localhost:5071/swagger`

---

## Not included yet

| Session | Scope |
|---|---|
| 02 | AutoMapper, generic repository, services |
| 03 | Middlewares |
| 04 | Authentication and filters |

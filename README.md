# Health Care System API

DEPI Round 5 — **Sessions 01–02**

ASP.NET Core Web API on **.NET 10** with **EF Core**, **SQL Server**, **AutoMapper** and **Swagger**.

---

## Session 01 — foundation

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

## Session 02 — repository, service and AutoMapper

### Layers

```text
Controller  →  Service  →  Repository  →  DbContext  →  SQL Server
                  ↓
             AutoMapper (Entity → DTO)
```

### Results — `Health Care System/Results`

| Type | Purpose |
|---|---|
| `Result` | success flag plus an error message |
| `GenericResult<T>` | the same, with a `Value` payload |
| `PaginatedList<T>` | one page of a result set |

### Repositories — `Health Care System/Repositories`

| Type | Purpose |
|---|---|
| `IGenericRepository<T>` | `GetAll`, `GetById`, `Create`, `Update`, `Delete` for any `BaseEntity` |
| `GenericRepository<T>` | the only implementation, registered as an open generic |
| `IAppointmentRepository` | `Appointment` has a composite key and no `Id`, so it gets its own contract |
| `AppointmentRepository` | resolves an appointment by `(PatientId, DoctorId, AppointmentDate)` and eager-loads both names |

### DTOs and profiles

| DTO | Profile |
|---|---|
| `GetPatientResponse` | `PatientProfile` |
| `GetDoctorResponse` | `DoctorProfile` |
| `GetAppointmentResponse` | `AppointmentProfile` (flattens `PatientName` and `DoctorName`) |

The DTOs never expose the audit columns (`IsDeleted`, `CreatedAt`, `CreatedBy`, …).

### Services — `Health Care System/Services`

`IPatientService`, `IDoctorService` and `IAppointmentService` return `GenericResult<T>`
instead of throwing, so the controller chooses `200` or `404`.

### Endpoints

| Method | Route |
|---|---|
| `GET` | `api/patients` |
| `GET` | `api/patients/{id}` |
| `GET` | `api/doctors` |
| `GET` | `api/doctors/{id}` |
| `GET` | `api/appointments` |
| `GET` | `api/appointments/{patientId}/{doctorId}?appointmentDate=...` |

A missing row answers `404` with the same envelope, for example:

```json
{ "value": null, "isSuccess": false, "isFailure": true, "error": "Patient with Id = 999 is not found." }
```

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
| 03 | Middlewares |
| 04 | Authentication and filters |

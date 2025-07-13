# Order Viewer – Full Stack App

A lightweight e-commerce order viewer app built with:

- **ASP.NET Core Web API** (.NET 8)
- **Blazor WebAssembly** frontend (.NET 9)
- **Entity Framework Core** (Code-first, MSSQL)
- **xUnit + Moq** for unit testing (.NET 8)

---

## 📦 Features

- ✅ View orders (ID, customer, status, total, created date)
- ✅ Filter by status, date range, and total range
- ✅ View order line items
- ✅ Mark orders as paid (no full reload)
- ✅ Live stats (count and grand total)
- ✅ Unit-tested service layer
- ✅ Filter form with reactive updates

---

## Prerequisites

- Visual Studio 2022+ with .NET 8 SDK
- Microsoft SQL Server (local or remote)

---

## 🚀 Setup & Run Instructions

### 1. **Clone the Repo**
```bash
git clone https://github.com/Dez1101/OrderViewer.git
```

### 2. Configure SQL Server
- Open appsettings.json file of OrderViewer.API and make changes to the connection string using the instructions after the string
```bash
"ConnectionStrings": {
  "OrderViewerConnectionStr": "Server=SQLSERERNAME;Database=OrderViewerDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
- NB: in the connection string above, please replace the word SQLSERERNAME with your actual sql server name that is on your local or server.
### How to Find Your SQL Server Name

- Open SSMS (SQL Server Management Studio).
- On the Connect to Server window, look at the Server name field.
- Copy the full server name exactly as shown — it might look like one of these: localhost, DESKTOP-ABC123\SQLEXPRESS or YOURCOMPUTERNAME\SQL2019
- Use this value in your connection string as the Server:
- 
### 3. **Open Solution in Visual Studio**

- Right-click the solution → Configure Startup Projects...

- Choose Multiple startup projects:

- OrderViewerAPI → Start

- OrderViewerUI → Start

- Save

### 4 Run the App**
- Click Start in Visual Studio

- Browser opens at:

-- Blazor: https://localhost:7160/orders

-- API: https://localhost:7193/swagger (optional)
### Use the following request body to test the post endpoint https://localhost:7193/api/Orders/filter in swagger or postman:

```bash
{
  "startDate": "2023-01-01T00:00:00",
  "endDate": "2025-12-31T00:00:00",
  "statuses": ["Pending", "Shipped", "cancelled", "Processing"],
  "minTotal": 100,
  "maxTotal": 1000,
  "sortBy": "createddate",
  "sortDirection": "desc"
}
```
### How to Run xUnit Tests in Visual Studio
- go to Build > Build Solution
- This restores NuGet packages and compiles the test project.
- Go to Test > Test Explorer (look for the word Test, between Debug and Analyze in VS top navbar)
- You should now see the list of tests from OrderViewerAPI.Tests
- Click Run All in the Test Explorer toolbar
- Or right-click on a specific test or class and select Run
- Test outcomes (✔ Passed, ❌ Failed) will appear in the Test Explorer
- For failed tests, you’ll see detailed error messages and stack traces

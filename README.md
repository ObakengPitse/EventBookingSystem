# EventEase Venue Booking System

The **EventEase Venue Booking System** is a cloud-based ASP.NET Core MVC web application designed to streamline the scheduling and management of event venues. Built for internal use by booking specialists, the platform ensures efficient handling of events, venues, and customer bookings while preventing scheduling conflicts and maintaining high data integrity.

## 🌐 Live Deployment
This application is hosted on **Azure App Service** with images stored on **Azure Blob Storage** and data maintained in **Azure SQL Database**.

---

## 🧩 Features

- **Venue Management**
  - Add, update, and delete venue records.
  - Upload venue images securely via Azure Blob Storage.
  - Prevent deletion of venues with existing bookings.

- **Event Management**
  - Add events before or after selecting an available venue.
  - Associate events with specific venues and dates.

- **Booking System**
  - Ensure bookings are conflict-free with date-time validation.
  - Assign unique Booking IDs and manage status.

- **Filtering**
  - Filter by date, venue, event type, customer name

- **Security**
  - Input validation and protection against SQL injection and XSS.

---

## 💻 Tech Stack

| Layer | Technology |
|-------|------------|
| Frontend | ASP.NET Razor Pages, Bootstrap |
| Backend | ASP.NET Core MVC, C# |
| ORM | Entity Framework Core |
| Database | Azure SQL Database |
| Image Storage | Azure Blob Storage |
| Hosting | Azure App Service |
| CI/CD | GitHub Actions |
| Monitoring | Azure Application Insights |

---

## ☁️ Azure Services Used

- **Azure App Service** – Web hosting for the application.
- **Azure SQL Database** – Central storage for events, venues, and bookings.
- **Azure Blob Storage** – Image storage for venue photos.
- **Azure Application Insights** – Monitoring, diagnostics, and logging.

---

## 📦 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/)
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)

### Clone the Repository

```bash
git clone https://github.com/ObakengPitse/EventBookingSystem.git
cd EventBookingSystem

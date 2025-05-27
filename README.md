# EgyEagles Vehicle Management System

[![.NET Core](https://img.shields.io/badge/.NET%20Core-9.0-blue)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

EgyEagles is a vehicle management system built with **.NET Core** and **Razor Pages**, designed to streamline the management of fleets, users, and company entities. It includes full authentication, authorization, and real-time vehicle tracking on an interactive map.

🔗 **Live Repo**: [https://github.com/DaliaHallabo/EgyEagles](https://github.com/DaliaHallabo/EgyEagles)

---

## 🚀 Features

- **Authentication & Authorization**: Secure login system with role-based access control.
- **Company Management**: Add and manage multiple companies.
- **Vehicle Management**: Register, update, and track vehicles.
- **User Management**: Assign users to companies with customizable permissions.
- **Permissions System**: Admin can define fine-grained access for each user.
- **Interactive Map**: Real-time map showing vehicle locations using mapping APIs.
- **Responsive UI**: Built with Razor Pages and modern styling for a smooth UX.

---

## 📸 Screenshots

![image](https://github.com/user-attachments/assets/4ec7ba94-6577-4e9b-806f-a0c19c132665)

![image](https://github.com/user-attachments/assets/a77adcfd-a85e-474c-b793-a3257b155918)

![image](https://github.com/user-attachments/assets/347fe05b-6f0e-4106-b4e2-6ae6c2682db0)

![image](https://github.com/user-attachments/assets/c179f8ec-dda3-4582-a6ae-8eed55b2c95f)

![image](https://github.com/user-attachments/assets/5c19e04c-9b2e-43f5-a0ab-b9141cc17813)

![image](https://github.com/user-attachments/assets/a709f03c-5525-44f3-a064-226161f43d73)

![image](https://github.com/user-attachments/assets/ae2e7fff-1cc4-4028-ae0d-e7403a6cdc54)

![image](https://github.com/user-attachments/assets/77489d60-03e9-4162-b6b2-547543d88eac)

---

## 🛠 Tech Stack

- **Backend**: ASP.NET Core
- **Frontend**: Razor Pages
- **Authentication**: Identity Framework
- **Database**: Entity Framework Core + SQL Server
---

## 📦 Installation

1. **Clone the repo**:

   ```bash
   git clone https://github.com/DaliaHallabo/EgyEagles.git
   cd EgyEagles
   ```

2. **Setup the database**:

   * Configure your connection string in `appsettings.json`.
   * Run EF migrations:

     ```bash
     dotnet ef database update
     ```

3. **Run the project**:

   ```bash
   dotnet run
   ```

4. Open your browser at: `https://localhost:5001` or `http://localhost:5000`
---

## 👩‍💻 Contributing

Contributions are welcome! Please fork the repo and submit a pull request.
---

## 📬 Contact

Created by [Dalia Hallabo](https://github.com/DaliaHallabo) – feel free to reach out for suggestions or collaboration!

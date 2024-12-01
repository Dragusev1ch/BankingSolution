# Banking Solution API

A simple REST API for managing basic banking operations, including account management, deposits, and fund transfers. This project is built with **C#** and is designed for ease of use, maintainability, and scalability.

## Features

### Account Management
- **Create a new account** with an initial balance.
- **Retrieve account details** using the account number.
- **List all accounts** available in the system.

### Account Transactions
- **Deposit funds** into an account.
- **Withdraw funds** from an account.
- **Transfer funds** between accounts.

### Additional Features
- Fully **RESTful API** design.
- Data is stored **in memory** during runtime (no database is required).
- Detailed **logging** for all operations.
- Comprehensive **unit tests** with **xUnit** for high code quality.
- Code is fully **commented** for better readability and maintainability.

## Tech Stack
- **Language:** C#
- **Framework:** ASP.NET Core
- **Testing Framework:** xUnit
- **Logging:** ASP.NET Core logging
- **API Documentation:** Swagger/OpenAPI

## Prerequisites
Before running the application, ensure the following tools are installed:
- [.NET SDK](https://dotnet.microsoft.com/download) (version 7.0 or later)
- [Postman](https://www.postman.com/) or any API testing tool (optional)

## Installation and Setup

### 1. Clone the Repository
First, clone the project repository to your local machine. If you are using **Visual Studio** or **Visual Studio Code**, you can simply open the project folder.

```bash
git clone https://github.com/your-username/banking-solution.git
```
Alternatively, you can download the project ZIP archive and extract it.

### 2. Open the Project
If you are using **Visual Studio** :

- Open **Visual Studio**.
- **Select File** > **Open** > **Project/Solution**.
- Navigate to the folder where the project was cloned and open the BankingSolution.sln file.

If you are using **Visual Studio Code**:

- Open **Visual Studio Code**.
- **Select File** > **Open Folder**.
- Navigate to the folder where the project was cloned and select it.

### 3. Build the Application
Once the project is loaded into your chosen IDE:

- In **Visual Studio**: Go to **Build** > **Build Solution**.
- In **Visual Studio Code**: The IDE should automatically detect the build settings and you can simply press F5 to build and run the application.

### 4. Run the Application
- In **Visual Studio**: Press F5 to start the application. The API will start, and you can view the output in the Output window.
- **In Visual Studio Code**: Press F5 to run the application. The API will start, and you can view the logs in the Terminal window.

The API will be available at https://localhost:5001 or http://localhost:5000.

### 5. Access Swagger UI (API Documentation)
Once the application is running, you can visit the following URL in your browser to access the API documentation:

- Swagger UI: https://localhost:5001/swagger

The Swagger UI will allow you to explore and test the available API endpoints easily.

### 6. Test the Endpoints
You can test the API using Swagger UI, Postman, or any HTTP client. Here are the endpoints available:

**Account Management**
- GET /api/accounts: List all accounts.
- GET /api/accounts/{accountNumber}: Get account details by account number.
- POST /api/accounts: Create a new account with an initial balance

**Account Transactions**
- POST /api/Transactions/deposit: Deposit funds into an account.
- POST /api/Transactions/withdraw: Withdraw funds from an account.
- POST /api/Transactions/transfer: Transfer funds between accounts.

### 7. Run Tests
If you want to ensure everything is functioning correctly, you can run the unit tests:

- In Visual Studio: Go to Test > Run All Tests.
- In Visual Studio Code: Use the integrated terminal to run dotnet test.

## Design and Implementation
- **In-Memory Storage**: The application stores all data in memory, so no external database setup is required.
- **Logging**: Integrated logging captures all API actions for debugging and audit purposes.
- **Modular Design**: The project is designed for easy future expansion, such as adding new transaction types or account features.
- **Testing**: Every functionality is covered by unit tests to maintain reliability and robustness.

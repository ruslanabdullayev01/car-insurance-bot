# 🚗 Telegram Bot for Car Insurance Sales

## 📌 Overview
This project is a **modular and scalable Telegram bot** for car insurance sales built with **.NET 8** following **Clean Architecture** principles.  
The bot guides users through document submission, OCR processing, price confirmation, and generates a dummy insurance policy in PDF format.  

It is designed with maintainability and testability in mind using **CQRS**, **MediatR**, **Repository and UnitOfWork pattern**, **Result Pattern** and **Dependency Injection**.

---

## 🎯 Features
✅ **Document upload & OCR** – Users upload **passport** and **vehicle registration certificate**, text is extracted via **Mindee API**  
✅ **Policy generation** – Creates and sends a PDF insurance policy to the user  
✅ **Database integration** – Stores all user actions, documents, policies, and logs in **PostgreSql**  
✅ **OpenAI Chat Support** – /chat command allows users to ask simple insurance-related questions

---

## 🏗️ Architecture
This bot is built using **Clean Architecture** and the **CQRS** pattern, ensuring clear separation of concerns:

- **API Layer**: Handles Telegram Bot API updates, controllers, middlewares, and program startup logic.  
- **Application Layer**: Contains **CQRS** logic (Commands, Queries, Handlers), DTOs, service/repository interfaces, and MediatR configuration.  
- **Domain Layer**: Core business layer — contains **Entities**, **Enums**, **Abstractions** and domain rules.  
- **Infrastructure Layer**: Responsible for **EF Core** (DbContext, Migrations), repositories, external integrations (Mindee OCR, OpenAI), PDF generation, and persistence logic.  

📂 **Project Structure**
```
src/
├── CarInsuranceBot.Api            # Telegram bot entry point
├── CarInsuranceBot.Application    # CQRS (Commands & Queries)
├── CarInsuranceBot.Domain         # Entities, Events, Business Logic
├── CarInsuranceBot.Infrastructure # DB, Mindee OCR, OpenAI, PDF generation
tests/
└── CarInsuranceBot.Tests          # xUnit unit tests
```

---

## 📥 Setup & Installation

### 1️⃣ Clone the repository
```bash
git clone https://github.com/ruslanabdullayev01/car-insurance-bot.git
cd car-insurance-bot
```

### 2️⃣ Configure environment variables
The bot uses **appsettings.json** for configuration.  
Here is the required structure:

```json
{
  "ConnectionStrings": {
    "Db": "Host=YOUR_HOST;Port=YOUR_PORT;Username=YOUR_USER;Password=YOUR_PASSWORD;Database=CarInsuranceBot;"
  },
  "SwaggerAuth": {
    "Username": "username",
    "Password": "password"
  },
  "OpenAi": {
    "ApiUrl": "https://api.openai.com/v1/chat/completions",
    "ApiKey": "your-api-key"
  },
  "TelegramBot": {
    "Token": "your-telegram-bot-token"
  },
  "Mindee": {
    "ApiKey": "your-mindee-api-key",
    "PasswordModelId": "your-password-model-id",
    "VehicleCertificateModelId": "your-vehicle-certificate-model-id"
  }
}
```

### 3️⃣ Run the database migrations
```bash
dotnet ef migrations add InitialCreate -p src/CarInsuranceBot.Infrastructure -s src/CarInsuranceBot.Api
dotnet ef database update -p src/CarInsuranceBot.Infrastructure -s src/CarInsuranceBot.Api
```

### 4️⃣ Start the bot
```bash
dotnet run --project src/CarInsuranceBot.Api
```

### 5️⃣ (Optional) Run with Docker
```bash
docker build -t car-insurance-bot .
docker run -p 5000:80 car-insurance-bot
```

---

## 🚀 CI/CD & Deployment
This project uses **GitHub Actions** for automatic deployment.  
When changes are pushed to the `master` branch, the workflow:
- Connects to the production server via SSH
- Pulls the latest code
- Rebuilds and restarts Docker containers

📂 Workflow file: `.github/workflows/autodeploy.yml`

---

## 🤖 Bot Commands
| Command     | Description                                                                 |
|-------------|-----------------------------------------------------------------------------|
| `/start`    | Starts the bot, creates or updates the user, and explains the process       |
| `/help`     | Shows help instructions                                                     |
| `/status`   | Displays the current step (passport photo, registration photo, completed)   |
| `/cancel`   | Clears all saved data for the user and restarts the process                 |
| `/viewocr`  | Shows OCR-extracted fields and asks if the user accepts the $100 payment    |
| `/chat`     | Opens a chat mode – user can ask insurance-related questions                |
| **yes/no**  | User response to confirm or reject the insurance purchase                   |

---

## 🗄️ Database Schema
**Tables used:**
- **Users** – stores TelegramUserId, full name, state (StateType), and related documents/policies.  
- **Documents** – stores user-uploaded files (passport, vehicle registration), including type and file path.  
- **ExtractedFields** – key-value pairs extracted from documents via OCR, linked to a specific User.  
- **Policies** – issued insurance policies with policy number, issue date, expiry date, and PDF file path.  
- **AuditLogs** – records every important action (Action, PerformedBy, old/new values).  
- **Errors** – logs system errors with message, stack trace, timestamp, and optional context.
- **Conversations** – OpenAI logs for audits

---

## 📜 License
MIT License – free to use and modify.  

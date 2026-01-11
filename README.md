# AI Appointment Booking Agent (.NET + Local LLM)
#📌 Project Overview

AI Appointment Booking Agent is a backend application built using ASP.NET Core, Entity Framework Core, SQL Server, and a local Large Language Model (LLM) powered by Ollama.

The application allows users to book medical appointments using natural language (e.g.,"Book a dentist appointment tomorrow at 11 AM for tooth pain").

The AI parses the user message, extracts structured appointment details, validates them against real database data, and saves the appointment if valid.

🚀 No cloud services required — the AI runs fully on your local machine.

#🧠 Key Features

✅ Natural language appointment booking
✅ Local AI (LLM) integration using Ollama
✅ AI hallucination prevention using database validation
✅ SQL Server + EF Core
✅ Clean layered architecture (Controller → Service → DB)
✅ Swagger UI for testing APIs
✅ Background-ready (can be extended with reminders)
------------------------------------------------------------------------------------------------------------------------------------------------------------

# 🏗️ Tech Stack
# Layer	        Technology
Backend	      ASP.NET Core Web API (.NET 8)
ORM	          Entity Framework Core
Database	    SQL Server
AI / LLM	    Ollama (Local)
Model Used	  llama3.2:1b (lightweight)
API Testing	  Swagger

# ------------------------------------------------------------------------------------------------------------------------------------------------------------
# 🧩 High-Level Architecture
User Input (Text)
      ↓
ChatController
      ↓
AiService (LLM Parsing)
      ↓
AppointmentService (Validation)
      ↓
KnowledgeService (DB facts)
      ↓
SQL Server (EF Core)
------------------------------------------------------------------------------------------------------------------------------------------------------------

🤖 Local LLM Setup (Ollama)
🔹 Step 1: Install Ollama

👉 Download and install from:
https://ollama.com

🔹 Step 2: Start Ollama Server

Open Command Prompt / PowerShell and run:
ollama serve

Verify Ollama is running:
curl http://localhost:11434/api/tags

🔹 Step 3: Pull the AI Model
ollama pull llama3.2:1b

This is a lightweight model suitable for local machines.

🛠️ Project Setup
🔹 Step 1: Clone the Repository
git clone <your-repo-url>
cd AiAppointmentAgent

🔹 Step 2: Configure Database

Update appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=AiAppointmentDb;Trusted_Connection=True;TrustServerCertificate=True"
}

🔹 Step 3: Apply EF Core Migrations

Open Package Manager Console:

Add-Migration InitialCreate
Update-Database

This will create:
Doctors
ClinicRules
Appointments
Seed data is added automatically.

▶️ Running the Application
dotnet run

Application will start at:
https://localhost:7093

Open Swagger:
https://localhost:7093/swagger

🧪 How to Test the Application
🔹 Swagger → POST /api/chat
Request
{
  "message": "Book a dentist appointment tomorrow at 11 AM for tooth pain"
}

Success Response
{
  "status": "SUCCESS",
  "message": "Appointment booked successfully"
}

❌ Validation Examples
Doctor Not Available
{
  "message": "Book a neurologist appointment tomorrow at 10 AM"
}


Response:
{
  "status": "FAILED",
  "message": "Doctor not available."
}

Slot Already Booked
Same request twice → second request fails.

# ------------------------------------------------------------------------------------------------------------------------------------------------------------

# 🧠 AI Prompt Strategy
Current date injected into prompt to avoid hallucinations
Strict JSON-only response enforced
Output validated against real DB data
Ollama response sanitized before deserialization

# ------------------------------------------------------------------------------------------------------------------------------------------------------------

# 📂 Project Structure
AiAppointmentAgent
│
├── Controllers
│   └── ChatController.cs
│
├── Services
│   ├── AiService.cs
│   ├── AppointmentService.cs
│   └── KnowledgeService.cs
│
├── Data
│   └── AppDbContext.cs
│
├── Models
│   ├── Appointment.cs
│   ├── Doctor.cs
│   └── ClinicRule.cs
│
├── DTOs
│   └── AppointmentDto.cs
│
├── Migrations
├── appsettings.json
└── Program.cs

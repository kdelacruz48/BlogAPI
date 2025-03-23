🚀 BlogAPI – .NET Core & PostgreSQL
A containerized .NET Core Web API for managing a blog, using PostgreSQL as the database, deployed on Railway.

📌 Features
✅ User authentication with JWT
✅ CRUD operations for blog posts & users
✅ PostgreSQL database integration
✅ Dockerized for easy deployment
✅ Hosted on Railway

🛠️ Tech Stack
Backend: .NET Core 7+, ASP.NET Web API

Database: PostgreSQL

Containerization: Docker

Hosting: Railway

Authentication: JWT

🚀 Getting Started
Prerequisites
Ensure you have the following installed:

.NET SDK

Docker

PostgreSQL (for local dev, or use Railway)

🔧 Setup
Clone the repository

sh
Copy
Edit
git clone https://github.com/yourusername/BlogAPI.git
cd BlogAPI
Set up environment variables

Create an .env file in the root directory and define:

ini
Copy
Edit
DefaultSQLConnection=your_local_connection_string
PrivateConnection=your_railway_connection_string
ApiSettings__Secret=your_jwt_secret
Run the API locally

sh
Copy
Edit
dotnet run --project BlogAPI
Run with Docker

sh
Copy
Edit
docker build -t blogapi .
docker run -p 8080:80 --env-file .env blogapi
Access Swagger UI
Open http://localhost:8080/swagger

🐳 Deploying to Railway
Push your code to GitHub

Connect your Railway project to the repo

Add environment variables in Railway settings

Deploy and access your API 🎉

📜 License
MIT License. Feel free to use and modify!


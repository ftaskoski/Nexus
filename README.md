# Nexus

A real-time social web application for messaging and connecting with friends.

## Tech Stack

- **Frontend**: Vue 3, TypeScript, Tailwind CSS
- **Backend**: .NET 8, SignalR, SQL Server
- **Authentication**: Cookie-based authentication

## Features

### Real-time Communication
- Live messaging with typing indicators
- Online/offline friend status
- Instant notifications for friend requests

### Social Features
- Add and manage friends
- Real-time friend request notifications
- Message history

### AI Integration
- Built-in AI chat using Hugging Face free models
- Conversational AI assistant

## Getting Started

### Prerequisites
- Node.js 16+
- .NET 8 SDK
- SQL Server

### Setup

**Frontend:**
```bash
npm install
npm run dev
```

**Backend:**
```bash
dotnet restore
# Execute SQL scripts from Schema folder on your SQL Server database
dotnet run
```

### Configuration

Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-sql-server-connection"
  }
}
```

# 🏘️ ConectaBairro

A cross-platform community app connecting Brazilian citizens to local resources, government programs, courses, and opportunities.

Built with **Uno Platform** (.NET 10) for Windows, Android, iOS, and WebAssembly.

## ✨ Features

- 🗺️ **Interactive Map** - Find nearby public services (health, education, social assistance)
- 📚 **Free Courses** - Browse SENAI, SENAC, PRONATEC programs
- 💬 **Community Forum** - Ask questions, share knowledge
- 🤖 **AI Assistant** - Get instant help powered by Google Gemini
- 🏆 **Gamification** - Earn XP and badges for engagement
- 🔔 **Live Alerts** - Community notifications and updates
- 👤 **User Profile** - Track progress and preferences

## 🚀 Quick Start

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/) (for backend)
- Visual Studio 2022 or VS Code with C# extension

### Run the App

```bash
# Clone the repository
git clone https://github.com/your-org/conectabairro.git
cd conectabairro

# Run on Windows Desktop
dotnet run -f net10.0-desktop

# Run on Android
dotnet run -f net10.0-android

# Run on Browser (WebAssembly)
dotnet run -f net10.0-browserwasm
```

### Run the Backend

```bash
cd backend
npm install
npm start
# API running at http://localhost:3000
```

## 📁 Project Structure

```
ConectaBairro/
├── Views/              # XAML pages
│   ├── DashboardPage   # Main home screen
│   ├── CoursesPage     # Course catalog
│   ├── MapPage         # Interactive map
│   ├── ForumPage       # Community forum
│   ├── AIChatPage      # AI assistant chat
│   ├── BadgesPage      # Achievements
│   └── ProfilePage     # User settings
├── ViewModels/         # MVVM view models
├── Models/             # Data models
├── Services/           # Business logic
│   ├── AIAssistantService
│   ├── DatabaseService
│   ├── NavigationService
│   └── ...
├── Converters/         # XAML value converters
├── Resources/          # Styles, colors, assets
├── backend/            # Node.js API
│   ├── routes/
│   │   ├── assistant.js  # AI endpoint
│   │   ├── users.js
│   │   └── ...
│   └── server.js
└── Platforms/          # Platform-specific code
```

## 🤖 AI Assistant

The app includes an AI-powered assistant using Google Gemini:

```bash
# Test the AI endpoint
curl -X POST http://localhost:3000/api/assistant/ask \
  -H "Content-Type: application/json" \
  -d '{"question": "How do I find courses near me?"}'
```

Features:
- Context-aware responses about app features
- Information about Brazilian social programs
- Conversation history support
- Fallback to local responses when offline

## 🛠️ Tech Stack

| Layer | Technology |
|-------|------------|
| Frontend | Uno Platform, XAML, C# |
| Backend | Node.js, Express |
| Database | SQLite (local), MongoDB (cloud) |
| AI | Google Gemini API |
| Maps | Google Maps Embed API |

## 📱 Screenshots

| Dashboard | Forum | AI Chat |
|-----------|-------|---------|
| Home with map and opportunities | Community discussions | AI assistant |

## 🔧 Configuration

### Environment Variables (Backend)

```env
PORT=3000
MONGODB_URI=mongodb://localhost:27017/conectabairro
AI_API_KEY=your_google_ai_key
```

### API Keys

- **Google Maps**: Configure in `DashboardPage.xaml.cs`
- **Google AI**: Configure in `backend/routes/assistant.js`

## 📄 License

MIT License - See [LICENSE](LICENSE) for details.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing`)
5. Open a Pull Request

## 📞 Support

- 📧 Email: support@conectabairro.com.br
- 💬 Forum: In-app community forum
- 🐛 Issues: GitHub Issues

---

Made with ❤️ for Brazilian communities

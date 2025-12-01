# 🏘️ ConectaBairro - Projeto Completo

## 📱 Visão Geral
App de mobilidade social que conecta comunidades com oportunidades de cursos, programas sociais e recursos locais.

## ✅ Funcionalidades Implementadas

### 🎨 Interface (Views)
| Página | Descrição | Status |
|--------|-----------|--------|
| OnboardingPage | Tela de boas-vindas com gradiente | ✅ |
| DashboardPage | Home com mapa, stats e oportunidades | ✅ |
| CoursesPage | Catálogo de cursos com filtros | ✅ |
| MapPage | Mapa do bairro com recursos | ✅ |
| BadgesPage | Sistema de conquistas e XP | ✅ |
| ProfilePage | Perfil completo com atividades | ✅ |
| OpportunitiesPage | APIs reais de programas | ✅ |
| NotificationsPage | Sistema de notificações | ✅ |
| MessagesPage | Chat P2P com comunidade | ✅ |
| AlertsPage | Alertas comunitários | ✅ |

### 🔧 Serviços (Services)
| Serviço | Função | Status |
|---------|--------|--------|
| NavigationService | Navegação entre páginas | ✅ |
| DatabaseService | SQLite persistência | ✅ |
| AnimationService | 9 animações prontas | ✅ |
| NotificationService | Sistema de notificações | ✅ |
| OpportunitiesService | APIs de oportunidades | ✅ |
| ThemeService | Dark/Light mode | ✅ |
| LocationService | Geolocalização | ✅ |

### 📊 Dados (MockData)
- 10 cursos reais (SENAI, SENAC, PRONATEC, SESC)
- 6 badges de conquistas
- 5 tipos de alertas
- 6 recursos locais

### 🎮 Gamificação
- Sistema de XP (450 pontos)
- Níveis (1-10)
- Badges desbloqueáveis
- Progresso visual

## 🚀 Como Executar

```powershell
cd C:\ConectaBairro
dotnet run -f net10.0-desktop
```

## 📁 Estrutura do Projeto

```
ConectaBairro/
├── Views/
│   ├── OnboardingPage.xaml
│   ├── DashboardPage.xaml
│   ├── CoursesPage.xaml
│   ├── MapPage.xaml
│   ├── BadgesPage.xaml
│   ├── ProfilePage.xaml
│   ├── OpportunitiesPage.xaml
│   ├── NotificationsPage.xaml
│   ├── MessagesPage.xaml
│   └── AlertsPage.xaml
├── ViewModels/
│   ├── DashboardViewModel.cs
│   ├── OpportunitiesViewModel.cs
│   └── AlertsViewModel.cs
├── Services/
│   ├── NavigationService.cs
│   ├── DatabaseService.cs
│   ├── AnimationService.cs
│   ├── NotificationService.cs
│   ├── OpportunitiesService.cs
│   ├── ThemeService.cs
│   └── LocationService.cs
├── Models/
│   ├── Course.cs
│   ├── Badge.cs
│   ├── Alert.cs
│   └── Notification.cs
├── MockData/
│   ├── MockCourses.cs
│   └── MockBadges.cs
└── Resources/
    └── Colors.xaml
```

## 🎯 Programas Sociais Incluídos

| Programa | Cursos | Bolsa |
|----------|--------|-------|
| Autonomia e Renda | 3 | R$ 750-1200/mês |
| PRONATEC | 3 | Gratuito |
| SENAC | 2 | R$ 1500-2500/mês |
| SESC | 2 | R$ 900-1100/mês |

## ✅ Funcionalidades Avançadas Implementadas

1. ✅ **Backend Real** - Node.js + MongoDB (backend/)
2. ✅ **Firebase** - Push notifications (Services/FirebaseService.cs)
3. ✅ **OAuth** - Login com Google/GitHub (Services/OAuthService.cs + Views/LoginPage.xaml)
4. ✅ **Google Maps** - Mapa integrado com WebView (Views/GoogleMapPage.xaml)
5. 🔜 **Deploy** - Google Play / App Store

## 🔐 Sistema de Autenticação OAuth

| Provider | Status | Funcionalidades |
|----------|--------|-----------------|
| Google | ✅ | Login, perfil, foto |
| GitHub | ✅ | Login, perfil, avatar |
| Visitante | ✅ | Acesso sem conta |

## 🗺️ Integração de Mapas

| Recurso | Implementação |
|---------|---------------|
| OpenStreetMap | Leaflet.js via WebView |
| Google Maps | API + WebView (com API key) |
| Geolocalização | Windows.Devices.Geolocation |
| Marcadores | Saúde, Educação, Social, Trabalho |
| Navegação | Abre Google Maps externo |

## 👨‍💻 Tecnologias

- **Framework**: Uno Platform
- **Linguagem**: C# / XAML
- **Database**: SQLite
- **Target**: .NET 10.0

---
**ConectaBairro** - Transformando comunidades através de oportunidades! 🚀

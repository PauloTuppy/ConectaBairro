# ConectaBairro API

Backend Node.js + MongoDB para o app ConectaBairro.

## 🚀 Quick Start

```bash
# Instalar dependências
npm install

# Configurar variáveis de ambiente
cp .env.example .env
# Edite .env com suas configurações

# Rodar em desenvolvimento
npm run dev

# Rodar em produção
npm start
```

## 📡 Endpoints

### Auth
- `POST /api/auth/register` - Criar conta
- `POST /api/auth/login` - Login
- `GET /api/auth/me` - Usuário atual

### Users
- `GET /api/users/profile` - Perfil
- `PUT /api/users/profile` - Atualizar perfil
- `POST /api/users/xp` - Adicionar XP
- `POST /api/users/badges` - Adicionar badge

### Opportunities
- `GET /api/opportunities` - Listar oportunidades
- `GET /api/opportunities/:id` - Detalhes
- `POST /api/opportunities/:id/enroll` - Inscrever-se

### Messages
- `GET /api/messages/conversations` - Conversas
- `GET /api/messages/:userId` - Mensagens com usuário
- `POST /api/messages` - Enviar mensagem

### Notifications
- `POST /api/notifications/send` - Enviar push
- `POST /api/notifications/broadcast` - Broadcast

## 🔧 Tecnologias

- Express.js
- MongoDB + Mongoose
- JWT Authentication
- bcryptjs
- Firebase Admin SDK

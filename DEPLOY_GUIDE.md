# 🚀 Guia de Deploy - ConectaBairro

## 📱 1. Google Play Store (Android)

### Pré-requisitos
- Conta de desenvolvedor Google Play ($25 única vez)
- Keystore para assinatura do app

### Gerar Keystore
```bash
keytool -genkey -v -keystore conectabairro.keystore -alias conectabairro -keyalg RSA -keysize 2048 -validity 10000
```

### Build Release Android
```bash
dotnet publish -f net10.0-android -c Release -p:AndroidKeyStore=true -p:AndroidSigningKeyStore=conectabairro.keystore -p:AndroidSigningKeyAlias=conectabairro -p:AndroidSigningKeyPass=SUA_SENHA -p:AndroidSigningStorePass=SUA_SENHA
```

### Arquivo gerado
`bin/Release/net10.0-android/publish/com.conectabairro.app-Signed.aab`

---

## 🍎 2. Apple App Store (iOS)

### Pré-requisitos
- Mac com Xcode instalado
- Conta Apple Developer ($99/ano)
- Certificados e Provisioning Profiles

### Build Release iOS
```bash
dotnet publish -f net10.0-ios -c Release
```

---

## 🖥️ 3. Microsoft Store (Windows)

### Build MSIX
```bash
dotnet publish -f net10.0-desktop -c Release -p:WindowsPackageType=MSIX
```

---

## 🌐 4. Backend (Node.js)

### Railway.app (Recomendado)
1. Acesse https://railway.app
2. Conecte seu GitHub
3. Selecione a pasta `backend`
4. Configure variáveis de ambiente:
   - `MONGODB_URI`
   - `JWT_SECRET`
   - `PORT=3000`

### Heroku
```bash
cd backend
heroku create conectabairro-api
heroku config:set MONGODB_URI="sua_uri_mongodb"
heroku config:set JWT_SECRET="sua_chave_secreta"
git push heroku main
```

---

## 🗄️ 5. MongoDB Atlas

1. Acesse https://cloud.mongodb.com
2. Crie um cluster gratuito (M0)
3. Configure Network Access (0.0.0.0/0 para produção)
4. Crie usuário do banco
5. Copie a Connection String

---

## 🔥 6. Firebase

### Cloud Messaging
1. Acesse https://console.firebase.google.com
2. Crie projeto "ConectaBairro"
3. Adicione app Android (com.conectabairro.app)
4. Baixe `google-services.json`
5. Habilite Cloud Messaging

### Configuração Android
Adicione ao `Platforms/Android/google-services.json`

---

## 🔐 7. OAuth Providers

### Google
1. https://console.cloud.google.com
2. APIs & Services > Credentials
3. Create OAuth 2.0 Client ID
4. Configure redirect URIs

### GitHub
1. https://github.com/settings/developers
2. New OAuth App
3. Configure callback URL

---

## ✅ Checklist Final

- [ ] Keystore Android criado e backup seguro
- [ ] Certificados iOS configurados
- [ ] Backend deployado
- [ ] MongoDB Atlas configurado
- [ ] Firebase configurado
- [ ] OAuth providers configurados
- [ ] Variáveis de ambiente em produção
- [ ] Testes em dispositivos reais
- [ ] Screenshots para stores
- [ ] Descrição e metadados das stores

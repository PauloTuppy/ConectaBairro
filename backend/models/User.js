const mongoose = require('mongoose');
const bcrypt = require('bcryptjs');

const userSchema = new mongoose.Schema({
  name: { type: String, required: true, trim: true },
  email: { type: String, required: true, unique: true, lowercase: true },
  password: { type: String, required: true, minlength: 6 },
  avatar: { type: String, default: '' },
  neighborhood: { type: String, default: '' },
  city: { type: String, default: '' },
  
  // Gamificação
  xp: { type: Number, default: 0 },
  level: { type: Number, default: 1 },
  badges: [{ type: String }],
  
  // Preferências
  preferences: {
    darkMode: { type: Boolean, default: false },
    notifications: { type: Boolean, default: true },
    language: { type: String, default: 'pt-BR' }
  },
  
  // OAuth
  oauthProvider: { type: String, enum: ['local', 'google', 'github'], default: 'local' },
  oauthId: { type: String },
  
  // Firebase
  fcmToken: { type: String },
  
  // Timestamps
  lastLogin: { type: Date },
  createdAt: { type: Date, default: Date.now },
  updatedAt: { type: Date, default: Date.now }
});

// Hash password antes de salvar
userSchema.pre('save', async function(next) {
  if (!this.isModified('password')) return next();
  this.password = await bcrypt.hash(this.password, 12);
  next();
});

// Comparar senha
userSchema.methods.comparePassword = async function(candidatePassword) {
  return await bcrypt.compare(candidatePassword, this.password);
};

// Calcular nível baseado em XP
userSchema.methods.calculateLevel = function() {
  this.level = Math.floor(this.xp / 1000) + 1;
  return this.level;
};

module.exports = mongoose.model('User', userSchema);

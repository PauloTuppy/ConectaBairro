const mongoose = require('mongoose');

const enrollmentSchema = new mongoose.Schema({
  user: { type: mongoose.Schema.Types.ObjectId, ref: 'User', required: true },
  opportunity: { type: mongoose.Schema.Types.ObjectId, ref: 'Opportunity', required: true },
  
  status: { 
    type: String, 
    enum: ['pending', 'approved', 'rejected', 'completed', 'cancelled'],
    default: 'pending'
  },
  
  // Progresso (para cursos)
  progress: { type: Number, default: 0, min: 0, max: 100 },
  
  // Notas e feedback
  notes: String,
  rating: { type: Number, min: 1, max: 5 },
  feedback: String,
  
  // XP ganho
  xpEarned: { type: Number, default: 0 },
  
  // Datas
  enrolledAt: { type: Date, default: Date.now },
  completedAt: Date,
  
  createdAt: { type: Date, default: Date.now },
  updatedAt: { type: Date, default: Date.now }
});

// Índice único para evitar inscrições duplicadas
enrollmentSchema.index({ user: 1, opportunity: 1 }, { unique: true });

module.exports = mongoose.model('Enrollment', enrollmentSchema);

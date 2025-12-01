const mongoose = require('mongoose');

const opportunitySchema = new mongoose.Schema({
  title: { type: String, required: true },
  description: { type: String, required: true },
  category: { 
    type: String, 
    enum: ['curso', 'emprego', 'social', 'saude', 'educacao'],
    required: true 
  },
  provider: { type: String, required: true }, // SENAI, SENAC, PRONATEC, etc.
  
  // Localização
  location: {
    address: String,
    city: String,
    state: String,
    coordinates: {
      lat: Number,
      lng: Number
    }
  },
  
  // Detalhes
  requirements: [String],
  benefits: [String],
  duration: String,
  modality: { type: String, enum: ['presencial', 'online', 'hibrido'] },
  
  // Vagas
  totalSlots: { type: Number, default: 0 },
  availableSlots: { type: Number, default: 0 },
  
  // Datas
  startDate: Date,
  endDate: Date,
  registrationDeadline: Date,
  
  // Links
  registrationUrl: String,
  imageUrl: String,
  
  // Status
  isActive: { type: Boolean, default: true },
  featured: { type: Boolean, default: false },
  
  // XP reward
  xpReward: { type: Number, default: 100 },
  
  createdAt: { type: Date, default: Date.now },
  updatedAt: { type: Date, default: Date.now }
});

// Index para busca
opportunitySchema.index({ title: 'text', description: 'text' });
opportunitySchema.index({ category: 1, isActive: 1 });
opportunitySchema.index({ 'location.city': 1 });

module.exports = mongoose.model('Opportunity', opportunitySchema);

const mongoose = require('mongoose');

const messageSchema = new mongoose.Schema({
  sender: { type: mongoose.Schema.Types.ObjectId, ref: 'User', required: true },
  receiver: { type: mongoose.Schema.Types.ObjectId, ref: 'User', required: true },
  content: { type: String, required: true },
  
  // Status
  read: { type: Boolean, default: false },
  readAt: Date,
  
  // Tipo de mensagem
  type: { 
    type: String, 
    enum: ['text', 'image', 'opportunity', 'system'],
    default: 'text'
  },
  
  // Referência a oportunidade (se compartilhada)
  opportunityRef: { type: mongoose.Schema.Types.ObjectId, ref: 'Opportunity' },
  
  createdAt: { type: Date, default: Date.now }
});

// Index para conversas
messageSchema.index({ sender: 1, receiver: 1, createdAt: -1 });

module.exports = mongoose.model('Message', messageSchema);

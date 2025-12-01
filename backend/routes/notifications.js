const express = require('express');
const router = express.Router();
const User = require('../models/User');
const { auth } = require('../middleware/auth');

// Simulação de Firebase Admin (em produção, usar firebase-admin real)
const sendPushNotification = async (fcmToken, title, body, data = {}) => {
  console.log(`📱 Push notification enviada para ${fcmToken}:`, { title, body, data });
  return { success: true, messageId: `msg_${Date.now()}` };
};

// POST /api/notifications/send
router.post('/send', auth, async (req, res) => {
  try {
    const { userId, title, body, data } = req.body;

    const targetUser = await User.findById(userId);
    if (!targetUser || !targetUser.fcmToken) {
      return res.status(400).json({ error: 'Usuário não encontrado ou sem token FCM' });
    }

    const result = await sendPushNotification(targetUser.fcmToken, title, body, data);
    res.json({ success: true, result });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao enviar notificação' });
  }
});

// POST /api/notifications/broadcast
router.post('/broadcast', auth, async (req, res) => {
  try {
    const { title, body, data, city } = req.body;

    const query = { fcmToken: { $exists: true, $ne: '' } };
    if (city) query.city = city;

    const users = await User.find(query).select('fcmToken');
    
    const results = await Promise.all(
      users.map(user => sendPushNotification(user.fcmToken, title, body, data))
    );

    res.json({ success: true, sent: results.length });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao enviar broadcast' });
  }
});

// POST /api/notifications/topic
router.post('/topic', async (req, res) => {
  try {
    const { topic, title, body, data } = req.body;
    
    console.log(`📢 Notificação enviada para tópico ${topic}:`, { title, body });
    res.json({ success: true, topic });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao enviar para tópico' });
  }
});

module.exports = router;

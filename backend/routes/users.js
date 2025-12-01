const express = require('express');
const router = express.Router();
const User = require('../models/User');
const { auth } = require('../middleware/auth');

// GET /api/users/profile
router.get('/profile', auth, async (req, res) => {
  res.json({ user: req.user });
});

// PUT /api/users/profile
router.put('/profile', auth, async (req, res) => {
  try {
    const updates = ['name', 'neighborhood', 'city', 'avatar', 'preferences'];
    const allowedUpdates = {};
    
    updates.forEach(field => {
      if (req.body[field] !== undefined) {
        allowedUpdates[field] = req.body[field];
      }
    });

    const user = await User.findByIdAndUpdate(
      req.user._id,
      { ...allowedUpdates, updatedAt: new Date() },
      { new: true }
    ).select('-password');

    res.json({ user });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao atualizar perfil' });
  }
});

// POST /api/users/xp
router.post('/xp', auth, async (req, res) => {
  try {
    const { amount, reason } = req.body;
    
    req.user.xp += amount;
    req.user.calculateLevel();
    await req.user.save();

    res.json({ xp: req.user.xp, level: req.user.level });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao adicionar XP' });
  }
});

// POST /api/users/badges
router.post('/badges', auth, async (req, res) => {
  try {
    const { badge } = req.body;
    
    if (!req.user.badges.includes(badge)) {
      req.user.badges.push(badge);
      await req.user.save();
    }

    res.json({ badges: req.user.badges });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao adicionar badge' });
  }
});

// PUT /api/users/fcm-token
router.put('/fcm-token', auth, async (req, res) => {
  try {
    const { fcmToken } = req.body;
    req.user.fcmToken = fcmToken;
    await req.user.save();
    res.json({ success: true });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao atualizar token FCM' });
  }
});

module.exports = router;

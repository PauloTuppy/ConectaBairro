const express = require('express');
const router = express.Router();
const Message = require('../models/Message');
const User = require('../models/User');
const { auth } = require('../middleware/auth');

// GET /api/messages/conversations
router.get('/conversations', auth, async (req, res) => {
  try {
    const userId = req.user._id;

    // Buscar últimas mensagens de cada conversa
    const conversations = await Message.aggregate([
      {
        $match: {
          $or: [{ sender: userId }, { receiver: userId }]
        }
      },
      { $sort: { createdAt: -1 } },
      {
        $group: {
          _id: {
            $cond: [{ $eq: ['$sender', userId] }, '$receiver', '$sender']
          },
          lastMessage: { $first: '$$ROOT' },
          unreadCount: {
            $sum: {
              $cond: [
                { $and: [{ $eq: ['$receiver', userId] }, { $eq: ['$read', false] }] },
                1,
                0
              ]
            }
          }
        }
      }
    ]);

    // Popular dados do usuário
    const populatedConversations = await User.populate(conversations, {
      path: '_id',
      select: 'name avatar'
    });

    res.json({ conversations: populatedConversations });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao buscar conversas' });
  }
});

// GET /api/messages/:userId
router.get('/:userId', auth, async (req, res) => {
  try {
    const messages = await Message.find({
      $or: [
        { sender: req.user._id, receiver: req.params.userId },
        { sender: req.params.userId, receiver: req.user._id }
      ]
    })
      .sort({ createdAt: 1 })
      .limit(100);

    // Marcar como lidas
    await Message.updateMany(
      { sender: req.params.userId, receiver: req.user._id, read: false },
      { read: true, readAt: new Date() }
    );

    res.json({ messages });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao buscar mensagens' });
  }
});

// POST /api/messages
router.post('/', auth, async (req, res) => {
  try {
    const { receiverId, content, type = 'text', opportunityRef } = req.body;

    const message = new Message({
      sender: req.user._id,
      receiver: receiverId,
      content,
      type,
      opportunityRef
    });
    await message.save();

    res.status(201).json({ message });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao enviar mensagem' });
  }
});

module.exports = router;

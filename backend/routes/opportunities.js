const express = require('express');
const router = express.Router();
const Opportunity = require('../models/Opportunity');
const Enrollment = require('../models/Enrollment');
const { auth } = require('../middleware/auth');

// GET /api/opportunities
router.get('/', async (req, res) => {
  try {
    const { category, city, search, page = 1, limit = 20 } = req.query;
    const query = { isActive: true };

    if (category) query.category = category;
    if (city) query['location.city'] = new RegExp(city, 'i');
    if (search) query.$text = { $search: search };

    const opportunities = await Opportunity.find(query)
      .sort({ featured: -1, createdAt: -1 })
      .skip((page - 1) * limit)
      .limit(parseInt(limit));

    const total = await Opportunity.countDocuments(query);

    res.json({ opportunities, total, page: parseInt(page), pages: Math.ceil(total / limit) });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao buscar oportunidades' });
  }
});

// GET /api/opportunities/:id
router.get('/:id', async (req, res) => {
  try {
    const opportunity = await Opportunity.findById(req.params.id);
    if (!opportunity) {
      return res.status(404).json({ error: 'Oportunidade não encontrada' });
    }
    res.json({ opportunity });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao buscar oportunidade' });
  }
});

// POST /api/opportunities/:id/enroll
router.post('/:id/enroll', auth, async (req, res) => {
  try {
    const opportunity = await Opportunity.findById(req.params.id);
    if (!opportunity) {
      return res.status(404).json({ error: 'Oportunidade não encontrada' });
    }

    const existingEnrollment = await Enrollment.findOne({
      user: req.user._id,
      opportunity: opportunity._id
    });

    if (existingEnrollment) {
      return res.status(400).json({ error: 'Você já está inscrito nesta oportunidade' });
    }

    const enrollment = new Enrollment({
      user: req.user._id,
      opportunity: opportunity._id
    });
    await enrollment.save();

    // Atualizar vagas disponíveis
    if (opportunity.availableSlots > 0) {
      opportunity.availableSlots--;
      await opportunity.save();
    }

    res.status(201).json({ enrollment, message: 'Inscrição realizada com sucesso!' });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao realizar inscrição' });
  }
});

// GET /api/opportunities/user/enrollments
router.get('/user/enrollments', auth, async (req, res) => {
  try {
    const enrollments = await Enrollment.find({ user: req.user._id })
      .populate('opportunity')
      .sort({ enrolledAt: -1 });

    res.json({ enrollments });
  } catch (error) {
    res.status(500).json({ error: 'Erro ao buscar inscrições' });
  }
});

module.exports = router;

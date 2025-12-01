const express = require('express');
const router = express.Router();
const axios = require('axios');

// AI API Configuration (Google AI / Gemini)
const AI_API_KEY = process.env.AI_API_KEY || 'AIzaSyD7-Kc4p-jCqnolaUlIk8x9XVSUuh5Kkro';
const AI_API_URL = 'https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent';

// System prompt for the assistant
const SYSTEM_PROMPT = `You are the AI tutor for the ConnectNeighborhood app (ConectaBairro).
Your role is to help users with:
- How to use the app features (map, courses, badges, profile)
- Information about Brazilian government social programs
- Finding local resources (health, education, employment, social services)

IMPORTANT RULES:
- NEVER promise benefits or guarantee eligibility
- Always recommend users verify information through official channels
- Be helpful, friendly, and use simple language
- If unsure, say so and suggest official sources
- Respond in the same language the user writes (Portuguese or English)

Available app features:
- Dashboard: Overview of opportunities and alerts
- Courses: Free professional courses (SENAI, SENAC, PRONATEC)
- Map: Find nearby public services
- Badges: Gamification with XP and achievements
- Profile: User settings and progress`;

// POST /api/assistant/ask
router.post('/ask', async (req, res) => {
  try {
    const { userId, threadId, question, context } = req.body;

    if (!question || question.trim().length === 0) {
      return res.status(400).json({
        success: false,
        error: 'Question is required'
      });
    }

    // Build conversation history
    const conversationParts = [];
    
    // Add context if provided
    if (context && Array.isArray(context)) {
      context.forEach(msg => {
        conversationParts.push({
          role: msg.role === 'assistant' ? 'model' : 'user',
          parts: [{ text: msg.content }]
        });
      });
    }

    // Add current question
    conversationParts.push({
      role: 'user',
      parts: [{ text: question }]
    });

    // Call Google AI (Gemini)
    const response = await axios.post(
      `${AI_API_URL}?key=${AI_API_KEY}`,
      {
        contents: conversationParts,
        systemInstruction: {
          parts: [{ text: SYSTEM_PROMPT }]
        },
        generationConfig: {
          temperature: 0.7,
          topK: 40,
          topP: 0.95,
          maxOutputTokens: 1024
        },
        safetySettings: [
          { category: 'HARM_CATEGORY_HARASSMENT', threshold: 'BLOCK_MEDIUM_AND_ABOVE' },
          { category: 'HARM_CATEGORY_HATE_SPEECH', threshold: 'BLOCK_MEDIUM_AND_ABOVE' },
          { category: 'HARM_CATEGORY_SEXUALLY_EXPLICIT', threshold: 'BLOCK_MEDIUM_AND_ABOVE' },
          { category: 'HARM_CATEGORY_DANGEROUS_CONTENT', threshold: 'BLOCK_MEDIUM_AND_ABOVE' }
        ]
      },
      {
        headers: {
          'Content-Type': 'application/json'
        }
      }
    );

    const answer = response.data.candidates?.[0]?.content?.parts?.[0]?.text || 
                   'Sorry, I could not generate a response. Please try again.';

    // TODO: Save to database if threadId is provided
    // await saveAssistantMessage({ userId, threadId, question, answer });

    res.json({
      success: true,
      answer: answer,
      conversationId: `conv_${Date.now()}`
    });

  } catch (error) {
    console.error('AI Assistant error:', error.response?.data || error.message);
    
    res.status(500).json({
      success: false,
      error: 'Failed to get AI response. Please try again later.',
      details: process.env.NODE_ENV === 'development' ? error.message : undefined
    });
  }
});

// GET /api/assistant/suggestions - Get suggested questions
router.get('/suggestions', (req, res) => {
  const suggestions = [
    { id: 1, text: 'How do I find courses near me?', category: 'app' },
    { id: 2, text: 'What is Bolsa Família and how to apply?', category: 'programs' },
    { id: 3, text: 'How do badges and XP work?', category: 'app' },
    { id: 4, text: 'Where can I find job opportunities?', category: 'services' },
    { id: 5, text: 'How to use the neighborhood map?', category: 'app' },
    { id: 6, text: 'What is PRONATEC?', category: 'programs' },
    { id: 7, text: 'How to register for free courses?', category: 'courses' },
    { id: 8, text: 'What social services are available?', category: 'services' }
  ];

  res.json({ success: true, suggestions });
});

module.exports = router;

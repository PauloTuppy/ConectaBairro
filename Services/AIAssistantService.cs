using System.Net.Http.Json;
using System.Text.Json;
using ConectaBairro.Models;

namespace ConectaBairro.Services;

/// <summary>
/// Service for AI Assistant integration
/// </summary>
public class AIAssistantService
{
    private static AIAssistantService? _instance;
    public static AIAssistantService Instance => _instance ??= new AIAssistantService();

    private readonly HttpClient _httpClient = new();
    private readonly string _backendUrl;
    private readonly List<ChatMessage> _conversationHistory = [];

    public event EventHandler<string>? MessageReceived;
    public event EventHandler<string>? ErrorOccurred;

    private AIAssistantService()
    {
        _backendUrl = "http://localhost:3000/api/assistant";
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    /// <summary>
    /// Ask the AI assistant a question
    /// </summary>
    public async Task<AskAssistantResponse> AskAsync(string question, string? threadId = null)
    {
        try
        {
            // Add user message to history
            _conversationHistory.Add(new ChatMessage { Role = "user", Content = question });

            var request = new AskAssistantRequest
            {
                UserId = GetCurrentUserId(),
                ThreadId = threadId,
                Question = question,
                Context = _conversationHistory.TakeLast(10).ToList() // Keep last 10 messages for context
            };

            var response = await _httpClient.PostAsJsonAsync($"{_backendUrl}/ask", request);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AskAssistantResponse>();
                if (result?.Success == true)
                {
                    // Add assistant response to history
                    _conversationHistory.Add(new ChatMessage { Role = "assistant", Content = result.Answer });
                    MessageReceived?.Invoke(this, result.Answer);
                    return result;
                }
            }

            // Fallback to local response if backend fails
            return await GetLocalResponseAsync(question);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AI Assistant error: {ex.Message}");
            ErrorOccurred?.Invoke(this, ex.Message);
            return await GetLocalResponseAsync(question);
        }
    }

    /// <summary>
    /// Get suggested questions
    /// </summary>
    public List<string> GetSuggestions()
    {
        return
        [
            "How do I find courses near me?",
            "What is Bolsa Família?",
            "How do badges and XP work?",
            "Where can I find job opportunities?",
            "How to use the neighborhood map?",
            "What is PRONATEC?",
            "How to register for free courses?",
            "What social services are available?"
        ];
    }

    /// <summary>
    /// Clear conversation history
    /// </summary>
    public void ClearHistory()
    {
        _conversationHistory.Clear();
    }

    /// <summary>
    /// Local fallback responses when backend is unavailable
    /// </summary>
    private Task<AskAssistantResponse> GetLocalResponseAsync(string question)
    {
        var lowerQuestion = question.ToLowerInvariant();
        string answer;

        if (lowerQuestion.Contains("course") || lowerQuestion.Contains("curso"))
        {
            answer = "📚 To find courses:\n\n1. Go to the **Courses** tab\n2. Browse available programs (SENAI, SENAC, PRONATEC)\n3. Filter by category or location\n4. Tap on a course to see details and enroll\n\nMany courses are free and some offer scholarships!";
        }
        else if (lowerQuestion.Contains("map") || lowerQuestion.Contains("mapa"))
        {
            answer = "🗺️ To use the Map:\n\n1. Go to the **Map** tab\n2. Allow location access for better results\n3. Use filters (Health, Education, Jobs, Social)\n4. Tap on markers to see details\n5. Use 'Directions' to get routes\n\nThe map shows public services near you!";
        }
        else if (lowerQuestion.Contains("badge") || lowerQuestion.Contains("xp"))
        {
            answer = "🏆 Badges & XP System:\n\n• **XP** - Earn points by completing activities\n• **Levels** - Progress from 1 to 10\n• **Badges** - Unlock achievements\n\nEarn XP by:\n- Completing courses (+100 XP)\n- Daily login (+10 XP)\n- Helping others (+50 XP)\n- Attending events (+75 XP)";
        }
        else if (lowerQuestion.Contains("bolsa") || lowerQuestion.Contains("benefit"))
        {
            answer = "💰 About Social Programs:\n\nI can provide general information, but for eligibility and registration:\n\n1. Visit **CRAS** (Social Assistance Center) near you\n2. Check the official website: gov.br\n3. Use the **Map** tab to find the nearest CRAS\n\n⚠️ Always verify through official channels!";
        }
        else if (lowerQuestion.Contains("job") || lowerQuestion.Contains("emprego") || lowerQuestion.Contains("trabalho"))
        {
            answer = "💼 Finding Jobs:\n\n1. Use the **Map** to find SINE offices\n2. Check **Opportunities** on the Dashboard\n3. Look for 'Autonomia e Renda' programs\n\nTips:\n- Keep your profile updated\n- Complete courses to improve skills\n- Check alerts for new openings";
        }
        else
        {
            answer = "👋 I'm here to help!\n\nI can assist with:\n• 📚 Finding courses\n• 🗺️ Using the map\n• 🏆 Understanding badges/XP\n• 💼 Job opportunities\n• 🏛️ Social services\n\nWhat would you like to know?";
        }

        _conversationHistory.Add(new ChatMessage { Role = "assistant", Content = answer });

        return Task.FromResult(new AskAssistantResponse
        {
            Success = true,
            Answer = answer,
            ConversationId = $"local_{DateTime.Now.Ticks}"
        });
    }

    private static string GetCurrentUserId()
    {
        return OAuthService.Instance.CurrentUser?.Id ?? "guest";
    }
}

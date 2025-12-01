namespace ConectaBairro.MockData;

using ConectaBairro.Models;

public static class MockBadges
{
    public static List<Badge> GetMockBadges()
    {
        return new()
        {
            new Badge { Name = "Primeiros Passos", Description = "Completou o primeiro curso", IconEmoji = "🚀", UnlockedDate = DateTime.Now.AddDays(-10) },
            new Badge { Name = "Estudioso", Description = "Completou 3 cursos", IconEmoji = "📚", UnlockedDate = null },
            new Badge { Name = "Mestre do Tempo", Description = "Entregou todas as tarefas no prazo", IconEmoji = "⏱️", UnlockedDate = DateTime.Now.AddDays(-5) },
            new Badge { Name = "Colaborador", Description = "Ajudou um colega no fórum", IconEmoji = "🤝", UnlockedDate = null },
            new Badge { Name = "Nota 10", Description = "Tirou nota máxima em uma avaliação", IconEmoji = "🌟", UnlockedDate = null },
            new Badge { Name = "Persistente", Description = "Estudou por 7 dias seguidos", IconEmoji = "🔥", UnlockedDate = null }
        };
    }
}

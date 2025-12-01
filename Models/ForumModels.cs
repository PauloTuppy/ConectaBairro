namespace ConectaBairro.Models;

/// <summary>
/// Forum Category - Top level organization
/// </summary>
public class ForumCategory
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "📁";
    public int Order { get; set; }
    public int TopicsCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Forum Topic - Groups related threads
/// </summary>
public class ForumTopic
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CategoryId { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string CreatedByUserId { get; set; } = "";
    public string CreatedByUserName { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;
    public bool IsLocked { get; set; }
    public bool IsPinned { get; set; }
    public List<string> Tags { get; set; } = [];
    public int ThreadsCount { get; set; }
    public int ViewsCount { get; set; }
}

/// <summary>
/// Forum Thread - A discussion within a topic
/// </summary>
public class ForumThread
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TopicId { get; set; } = "";
    public string Title { get; set; } = "";
    public string CreatedByUserId { get; set; } = "";
    public string CreatedByUserName { get; set; } = "";
    public string CreatedByUserAvatar { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;
    public int ViewsCount { get; set; }
    public int PostsCount { get; set; }
    public bool IsPinned { get; set; }
    public bool IsClosed { get; set; }
    public bool HasAIResponse { get; set; }
}

/// <summary>
/// Forum Post - Individual message in a thread
/// </summary>
public class ForumPost
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ThreadId { get; set; } = "";
    public string? ParentPostId { get; set; }
    public string AuthorUserId { get; set; } = "";
    public string AuthorUserName { get; set; } = "";
    public string AuthorUserAvatar { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EditedAt { get; set; }
    public bool IsFromAssistant { get; set; }
    public int UpvotesCount { get; set; }
    public int DownvotesCount { get; set; }
    public bool IsAcceptedAnswer { get; set; }
}

/// <summary>
/// AI Assistant Conversation
/// </summary>
public class AssistantConversation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = "";
    public string? ThreadId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;
    public List<AssistantMessage> Messages { get; set; } = [];
}

/// <summary>
/// Individual message in AI conversation
/// </summary>
public class AssistantMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ConversationId { get; set; } = "";
    public MessageSender Sender { get; set; }
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Metadata { get; set; }
}

public enum MessageSender
{
    User,
    Assistant,
    System
}

/// <summary>
/// Request to AI Assistant
/// </summary>
public class AskAssistantRequest
{
    public string UserId { get; set; } = "";
    public string? ThreadId { get; set; }
    public string Question { get; set; } = "";
    public List<ChatMessage>? Context { get; set; }
}

public class ChatMessage
{
    public string Role { get; set; } = "user";
    public string Content { get; set; } = "";
}

/// <summary>
/// Response from AI Assistant
/// </summary>
public class AskAssistantResponse
{
    public bool Success { get; set; }
    public string Answer { get; set; } = "";
    public string? Error { get; set; }
    public string? ConversationId { get; set; }
}

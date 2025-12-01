using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using ConectaBairro.Services;

namespace ConectaBairro.Views;

public sealed partial class AIChatPage : Page
{
    private bool _isWaitingForResponse;

    public AIChatPage()
    {
        InitializeComponent();
    }

    private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();

    private void ClearChat_Click(object sender, RoutedEventArgs e)
    {
        AIAssistantService.Instance.ClearHistory();
        MessagesPanel.Children.Clear();
        AddWelcomeMessage();
    }

    private void Suggestion_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string question)
        {
            MessageInput.Text = question;
            SendMessage();
        }
    }

    private void MessageInput_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter && !_isWaitingForResponse)
        {
            SendMessage();
            e.Handled = true;
        }
    }

    private void Send_Click(object sender, RoutedEventArgs e)
    {
        if (!_isWaitingForResponse)
        {
            SendMessage();
        }
    }

    private async void SendMessage()
    {
        var question = MessageInput.Text?.Trim();
        if (string.IsNullOrEmpty(question)) return;

        // Hide suggestions after first message
        SuggestionsPanel.Visibility = Visibility.Collapsed;

        // Add user message
        AddUserMessage(question);
        MessageInput.Text = "";

        // Show typing indicator
        _isWaitingForResponse = true;
        StatusText.Text = "Typing...";
        SendButton.IsEnabled = false;
        var typingIndicator = AddTypingIndicator();

        try
        {
            // Get AI response
            var response = await AIAssistantService.Instance.AskAsync(question);

            // Remove typing indicator
            MessagesPanel.Children.Remove(typingIndicator);

            if (response.Success)
            {
                AddAssistantMessage(response.Answer);
            }
            else
            {
                AddAssistantMessage("Sorry, I couldn't process your request. Please try again.");
            }
        }
        catch (System.Exception ex)
        {
            MessagesPanel.Children.Remove(typingIndicator);
            AddAssistantMessage($"Error: {ex.Message}");
        }
        finally
        {
            _isWaitingForResponse = false;
            StatusText.Text = "Online • Ready to help";
            SendButton.IsEnabled = true;
            ScrollToBottom();
        }
    }

    private void AddUserMessage(string text)
    {
        var border = new Border
        {
            Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 99, 102, 241)),
            CornerRadius = new CornerRadius(16, 16, 4, 16),
            Padding = new Thickness(14),
            MaxWidth = 320,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 4, 0, 4)
        };

        var textBlock = new TextBlock
        {
            Text = text,
            FontSize = 14,
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.White),
            TextWrapping = TextWrapping.Wrap
        };

        border.Child = textBlock;
        MessagesPanel.Children.Add(border);
        ScrollToBottom();
    }

    private void AddAssistantMessage(string text)
    {
        var border = new Border
        {
            Background = new SolidColorBrush(Microsoft.UI.Colors.White),
            CornerRadius = new CornerRadius(16, 16, 16, 4),
            Padding = new Thickness(14),
            MaxWidth = 320,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 4, 0, 4)
        };

        var stack = new StackPanel { Spacing = 8 };

        // Header with icon
        var header = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        var iconBorder = new Border
        {
            Width = 24,
            Height = 24,
            CornerRadius = new CornerRadius(12),
            Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 238, 242, 255))
        };
        iconBorder.Child = new TextBlock
        {
            Text = "🤖",
            FontSize = 12,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        header.Children.Add(iconBorder);
        header.Children.Add(new TextBlock
        {
            Text = "AI Assistant",
            FontSize = 11,
            FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 99, 102, 241)),
            VerticalAlignment = VerticalAlignment.Center
        });
        stack.Children.Add(header);

        // Message content
        var textBlock = new TextBlock
        {
            Text = text,
            FontSize = 14,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 31, 41, 55)),
            TextWrapping = TextWrapping.Wrap
        };
        stack.Children.Add(textBlock);

        border.Child = stack;
        MessagesPanel.Children.Add(border);
        ScrollToBottom();
    }

    private Border AddTypingIndicator()
    {
        var border = new Border
        {
            Background = new SolidColorBrush(Microsoft.UI.Colors.White),
            CornerRadius = new CornerRadius(16, 16, 16, 4),
            Padding = new Thickness(14),
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 4, 0, 4)
        };

        var stack = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
        for (int i = 0; i < 3; i++)
        {
            stack.Children.Add(new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 156, 163, 175))
            });
        }

        border.Child = stack;
        MessagesPanel.Children.Add(border);
        ScrollToBottom();
        return border;
    }

    private void AddWelcomeMessage()
    {
        var border = new Border
        {
            Background = new SolidColorBrush(Microsoft.UI.Colors.White),
            CornerRadius = new CornerRadius(16, 16, 16, 4),
            Padding = new Thickness(14),
            MaxWidth = 320,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        var stack = new StackPanel { Spacing = 8 };
        stack.Children.Add(new TextBlock
        {
            Text = "Hello! 👋 I'm your AI assistant. How can I help you today?",
            FontSize = 14,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 31, 41, 55)),
            TextWrapping = TextWrapping.Wrap
        });

        border.Child = stack;
        MessagesPanel.Children.Add(border);

        // Re-add suggestions
        SuggestionsPanel.Visibility = Visibility.Visible;
    }

    private void ScrollToBottom()
    {
        ChatScrollViewer.UpdateLayout();
        ChatScrollViewer.ChangeView(null, ChatScrollViewer.ScrollableHeight, null);
    }
}

// Helper class for Ellipse
public class Ellipse : Microsoft.UI.Xaml.Shapes.Ellipse { }

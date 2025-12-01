using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media; // For CompositeTransform
using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace ConectaBairro.Services;

public static class AnimationService
{
    // Utility to ensure CompositeTransform is available
    private static CompositeTransform GetOrCreateCompositeTransform(UIElement element)
    {
        if (element.RenderTransform is not CompositeTransform transform)
        {
            transform = new CompositeTransform();
            element.RenderTransform = transform;
        }
        return transform;
    }

    // Helper method to run a Storyboard and await its completion
    private static Task RunStoryboardAsync(Storyboard storyboard)
    {
        var tcs = new TaskCompletionSource();
        storyboard.Completed += (s, e) => tcs.TrySetResult();
        storyboard.Begin();
        return tcs.Task;
    }

    public static Task FadeIn(UIElement element, double durationSeconds = 0.3)
    {
        var storyboard = new Storyboard();
        var animation = new DoubleAnimation
        {
            From = 0.0,
            To = 1.0,
            Duration = new Duration(TimeSpan.FromSeconds(durationSeconds))
        };
        Storyboard.SetTarget(animation, element);
        Storyboard.SetTargetProperty(animation, "Opacity");
        storyboard.Children.Add(animation);
        return RunStoryboardAsync(storyboard); // Use helper
    }

    public static Task FadeOut(UIElement element, double durationSeconds = 0.3)
    {
        var storyboard = new Storyboard();
        var animation = new DoubleAnimation
        {
            From = 1.0,
            To = 0.0,
            Duration = new Duration(TimeSpan.FromSeconds(durationSeconds))
        };
        Storyboard.SetTarget(animation, element);
        Storyboard.SetTargetProperty(animation, "Opacity");
        storyboard.Children.Add(animation);
        return RunStoryboardAsync(storyboard); // Use helper
    }

    public static Task Rotate(UIElement element, double fromAngle = 0, double toAngle = 360, double durationSeconds = 0.5)
    {
        var storyboard = new Storyboard();
        var transformGroup = GetOrCreateCompositeTransform(element);
        element.RenderTransformOrigin = new Point(0.5, 0.5); // Rotate around center

        var animation = new DoubleAnimation
        {
            From = fromAngle,
            To = toAngle,
            Duration = new Duration(TimeSpan.FromSeconds(durationSeconds))
        };
        Storyboard.SetTarget(animation, transformGroup);
        Storyboard.SetTargetProperty(animation, "Rotation");
        storyboard.Children.Add(animation);
        return RunStoryboardAsync(storyboard); // Use helper
    }

    public static Task Scale(UIElement element, double fromScale = 1.0, double toScale = 1.2, double durationSeconds = 0.3)
    {
        var storyboard = new Storyboard();
        var transformGroup = GetOrCreateCompositeTransform(element);
        element.RenderTransformOrigin = new Point(0.5, 0.5);

        var scaleX = new DoubleAnimation
        {
            From = fromScale,
            To = toScale,
            Duration = new Duration(TimeSpan.FromSeconds(durationSeconds))
        };
        Storyboard.SetTarget(scaleX, transformGroup);
        Storyboard.SetTargetProperty(scaleX, "ScaleX");
        storyboard.Children.Add(scaleX);

        var scaleY = new DoubleAnimation
        {
            From = fromScale,
            To = toScale,
            Duration = new Duration(TimeSpan.FromSeconds(durationSeconds))
        };
        Storyboard.SetTarget(scaleY, transformGroup);
        Storyboard.SetTargetProperty(scaleY, "ScaleY");
        storyboard.Children.Add(scaleY);
        return RunStoryboardAsync(storyboard); // Use helper
    }

    public static async Task Bounce(UIElement element, double bounceHeight = -20, double durationSeconds = 0.4)
    {
        var storyboard = new Storyboard();
        var transformGroup = GetOrCreateCompositeTransform(element);

        var animation = new DoubleAnimationUsingKeyFrames
        {
            Duration = new Duration(TimeSpan.FromSeconds(durationSeconds))
        };
        // Calculate KeyTime from percentage
        var halfDuration = TimeSpan.FromSeconds(durationSeconds / 2);
        var fullDuration = TimeSpan.FromSeconds(durationSeconds);

        animation.KeyFrames.Add(new SplineDoubleKeyFrame { KeyTime = TimeSpan.Zero, Value = 0 });
        animation.KeyFrames.Add(new SplineDoubleKeyFrame { KeyTime = halfDuration, Value = bounceHeight, KeySpline = new KeySpline { ControlPoint1 = new Point(0.6, 0), ControlPoint2 = new Point(0.9, 0) } });
        animation.KeyFrames.Add(new SplineDoubleKeyFrame { KeyTime = fullDuration, Value = 0, KeySpline = new KeySpline { ControlPoint1 = new Point(0.1, 1), ControlPoint2 = new Point(0.4, 1) } });

        Storyboard.SetTarget(animation, transformGroup);
        Storyboard.SetTargetProperty(animation, "TranslateY");
        storyboard.Children.Add(animation);
        await RunStoryboardAsync(storyboard); // Use helper
    }

    public static Task Pulse(UIElement element, double scaleFactor = 1.1, double durationSeconds = 0.5)
    {
        var storyboard = new Storyboard();
        var transformGroup = GetOrCreateCompositeTransform(element);
        element.RenderTransformOrigin = new Point(0.5, 0.5);

        var halfDuration = TimeSpan.FromSeconds(durationSeconds / 2);

        var scaleUpX = new DoubleAnimation { From = 1.0, To = scaleFactor, Duration = new Duration(halfDuration) };
        var scaleUpY = new DoubleAnimation { From = 1.0, To = scaleFactor, Duration = new Duration(halfDuration) };
        var scaleDownX = new DoubleAnimation { From = scaleFactor, To = 1.0, Duration = new Duration(halfDuration), BeginTime = halfDuration };
        var scaleDownY = new DoubleAnimation { From = scaleFactor, To = 1.0, Duration = new Duration(halfDuration), BeginTime = halfDuration };

        Storyboard.SetTarget(scaleUpX, transformGroup); Storyboard.SetTargetProperty(scaleUpX, "ScaleX");
        Storyboard.SetTarget(scaleUpY, transformGroup); Storyboard.SetTargetProperty(scaleUpY, "ScaleY");
        Storyboard.SetTarget(scaleDownX, transformGroup); Storyboard.SetTargetProperty(scaleDownX, "ScaleX");
        Storyboard.SetTarget(scaleDownY, transformGroup); Storyboard.SetTargetProperty(scaleDownY, "ScaleY");

        storyboard.Children.Add(scaleUpX); storyboard.Children.Add(scaleUpY);
        storyboard.Children.Add(scaleDownX); storyboard.Children.Add(scaleDownY);
        return RunStoryboardAsync(storyboard); // Use helper
    }

    public static Task ConfettiAsync(UIElement parentContainer) // Changed to UIElement
    {
        System.Diagnostics.Debug.WriteLine("Confetti animation requested. (Placeholder)");
        // A full confetti animation would involve creating many small UI elements and animating them.
        // For now, this is a placeholder.
        return Task.CompletedTask;
    }

    public static Task XPGainAsync(UIElement parentContainer, int xpAmount) // Changed to UIElement
    {
        System.Diagnostics.Debug.WriteLine($"XP Gain animation requested: +{xpAmount} XP. (Placeholder)");
        // A full XP gain animation would involve creating a text element, animating its position and opacity.
        // For now, this is a placeholder.
        return Task.CompletedTask;
    }

    public static Task BadgeUnlockAsync(UIElement badgeElement)
    {
        // Simple rotation to simulate "spinning"
        return Rotate(badgeElement, fromAngle: 0, toAngle: 360, durationSeconds: 0.8);
    }
    
    public static Task Slide(UIElement element, double fromX = -100, double toX = 0, double durationSeconds = 0.3)
    {
        var storyboard = new Storyboard();
        var transformGroup = GetOrCreateCompositeTransform(element);

        var animation = new DoubleAnimation
        {
            From = fromX,
            To = toX,
            Duration = new Duration(TimeSpan.FromSeconds(durationSeconds))
        };
        Storyboard.SetTarget(animation, transformGroup);
        Storyboard.SetTargetProperty(animation, "TranslateX");
        storyboard.Children.Add(animation);
        return RunStoryboardAsync(storyboard); // Use helper
    }

    public static async Task Flip(UIElement element, double durationSeconds = 0.6)
    {
        // Simulate a flip effect using ScaleX (2D alternative to 3D rotation)
        var storyboard = new Storyboard();
        var transformGroup = GetOrCreateCompositeTransform(element);
        element.RenderTransformOrigin = new Point(0.5, 0.5);

        var halfDuration = TimeSpan.FromSeconds(durationSeconds / 2);
        var fullDuration = TimeSpan.FromSeconds(durationSeconds);

        // Animate ScaleX: 1 -> 0 -> 1 (creates flip illusion)
        var animation = new DoubleAnimationUsingKeyFrames
        {
            Duration = new Duration(fullDuration)
        };
        
        animation.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = TimeSpan.Zero, Value = 1.0 });
        animation.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = halfDuration, Value = 0.0 });
        animation.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = fullDuration, Value = 1.0 });

        Storyboard.SetTarget(animation, transformGroup);
        Storyboard.SetTargetProperty(animation, "ScaleX");
        storyboard.Children.Add(animation);
        await RunStoryboardAsync(storyboard);
    }
}
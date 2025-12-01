using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;

namespace ConectaBairro.Views;

public sealed partial class AnimationsTestPage : Page
{
    public AnimationsTestPage()
    {
        InitializeComponent();
    }

    private async void FadeIn_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.FadeIn(FadeBox);
    }

    private async void FadeOut_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.FadeOut(FadeBox);
    }

    private async void Rotate_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.Rotate(RotateBox);
    }

    private async void ScaleUp_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.Scale(ScaleBox, fromScale: 1.0, toScale: 1.3);
    }

    private async void ScaleDown_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.Scale(ScaleBox, fromScale: 1.3, toScale: 1.0);
    }

    private async void Bounce_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.Bounce(BounceBox);
    }

    private async void Pulse_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.Pulse(PulseBox);
    }

    private async void SlideRight_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.Slide(SlideBox, fromX: 0, toX: 100);
    }

    private async void SlideLeft_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.Slide(SlideBox, fromX: 100, toX: 0);
    }

    private async void BadgeUnlock_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.BadgeUnlockAsync(BadgeBox);
    }

    private async void Flip_Click(object sender, RoutedEventArgs e)
    {
        await AnimationService.Flip(FlipBox);
    }
}

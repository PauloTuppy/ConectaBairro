using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace ConectaBairro.Models;

public partial class JourneyPhase : ObservableObject
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public int CurrentProgress { get; set; } = 0; // Percentage 0-100
    public int TargetProgress { get; set; } = 100; // Percentage 0-100 (or specific count)
    public int XPReward { get; set; } = 0;
    public string IconEmoji { get; set; } = "✨";

    public bool IsCompleted => CurrentProgress >= TargetProgress;
    public double ProgressRatio => (double)CurrentProgress / TargetProgress;
}

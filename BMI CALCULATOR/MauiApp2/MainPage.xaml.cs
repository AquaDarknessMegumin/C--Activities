using System.Globalization;
using Microsoft.Maui.Graphics;

namespace MauiApp2;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCalculateClicked(object? sender, EventArgs e)
    {
        // Reset state
        MessageLabel.Text = string.Empty;
        ResultCard.Opacity = 0;
        ResultCard.IsVisible = false;

        if (!TryParsePositiveDouble(HeightEntry.Text, out var heightCm) ||
            !TryParsePositiveDouble(WeightEntry.Text, out var weightKg))
        {
            ShowValidationError("Please enter valid positive numbers for height and weight.");
            return;
        }

        if (heightCm is < 50 or > 300)
        {
            ShowValidationError("Height should be between 50 cm and 300 cm.");
            return;
        }

        if (weightKg is < 10 or > 500)
        {
            ShowValidationError("Weight should be between 10 kg and 500 kg.");
            return;
        }

        var heightMeters = heightCm / 100.0;
        var bmi = weightKg / (heightMeters * heightMeters);
        var categoryInfo = GetBmiCategoryInfo(bmi);

        BmiValueLabel.Text = $"{bmi:F1}";
        CategoryLabel.Text = categoryInfo.Name;
        CategoryLabel.TextColor = categoryInfo.Color;
        CategoryBorder.Stroke = categoryInfo.Color;
        MessageLabel.Text = categoryInfo.Message;
        BmiIndicator.Color = categoryInfo.Color;

        // Calculate progress bar width based on a max BMI of 40 for visual purposes
        var progressContainerWidth = ProgressBarContainer.Width;
        if (progressContainerWidth <= 0) progressContainerWidth = 250; // Fallback if not laid out yet

        var normalizedBmi = Math.Min(Math.Max(bmi, 15), 40);
        var progressPercentage = (normalizedBmi - 15) / 25.0; // 15 to 40 range
        var targetWidth = progressContainerWidth * progressPercentage;

        // Ensure ResultCard is visible before animating
        ResultCard.IsVisible = true;
        
        // Setup initial state for animation
        BmiIndicator.WidthRequest = 0;
        ResultCard.TranslationY = 20;

        // Animate in
        await Task.WhenAll(
            ResultCard.FadeTo(1, 400, Easing.CubicOut),
            ResultCard.TranslateTo(0, 0, 400, Easing.CubicOut)
        );

        // Animate the progress bar
        BmiIndicator.Animate("ProgressBarAnimation", 
            new Animation(v => BmiIndicator.WidthRequest = v, 0, targetWidth), 
            16, 800, Easing.CubicInOut);

        SemanticScreenReader.Announce($"BMI {bmi:F1}. Category {categoryInfo.Name}.");
    }

    private static bool TryParsePositiveDouble(string? input, out double value)
    {
        var normalized = input?.Trim().Replace(',', '.');

        if (double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            return value > 0;

        value = 0;
        return false;
    }

    private class BmiCategoryInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Color Color { get; set; } = Colors.White;
    }

    private static BmiCategoryInfo GetBmiCategoryInfo(double bmi) => bmi switch
    {
        < 18.5 => new BmiCategoryInfo { Name = "Underweight", Message = "You are below the healthy weight range.", Color = Color.FromArgb("#3B82F6") },
        < 25.0 => new BmiCategoryInfo { Name = "Normal Weight", Message = "You have a healthy body weight. Good job!", Color = Color.FromArgb("#10B981") },
        < 30.0 => new BmiCategoryInfo { Name = "Overweight", Message = "You are slightly above the healthy weight range.", Color = Color.FromArgb("#F59E0B") },
        _ => new BmiCategoryInfo { Name = "Obesity", Message = "You are in the obesity range. Please consult a doctor.", Color = Color.FromArgb("#EF4444") }
    };

    private void ShowValidationError(string message)
    {
        ResultCard.IsVisible = true;
        ResultCard.Opacity = 1;
        
        BmiValueLabel.Text = "--";
        CategoryLabel.Text = "Error";
        CategoryLabel.TextColor = Color.FromArgb("#EF4444");
        CategoryBorder.Stroke = Color.FromArgb("#EF4444");
        MessageLabel.Text = message;
        BmiIndicator.WidthRequest = 0;
        
        SemanticScreenReader.Announce(message);
    }
}

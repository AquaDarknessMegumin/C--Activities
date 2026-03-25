using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;

namespace Midterms;

public partial class MainPage : ContentPage
{
    private string currentInput = "";
    private bool isResultDisplayed = false;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnNumberClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string pressedNumber = button.Text;

        if (isResultDisplayed)
        {
            currentInput = "";
            isResultDisplayed = false;
        }

        if (pressedNumber == ".")
        {
            // Prevent multiple decimals in the current operand
            string[] parts = currentInput.Split('+', '−', '×', '÷');
            string currentOperand = parts[parts.Length - 1];

            if (currentOperand.Contains("."))
                return;

            if (string.IsNullOrEmpty(currentOperand))
                currentInput += "0";
        }

        currentInput += pressedNumber;
        UpdateDisplay();
    }

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string op = button.Text;

        isResultDisplayed = false;

        if (string.IsNullOrEmpty(currentInput))
            return;

        char lastChar = currentInput[currentInput.Length - 1];
        if (IsOperator(lastChar))
        {
            // Replace the last operator with the new one
            currentInput = currentInput.Substring(0, currentInput.Length - 1) + op;
        }
        else
        {
            currentInput += op;
        }

        UpdateDisplay();
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        currentInput = "";
        InputDisplay.Text = "";
        ResultDisplay.Text = "0";
        isResultDisplayed = false;
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (isResultDisplayed) return;

        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
        }
    }

    private void OnCalculateClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(currentInput))
            return;

        char lastChar = currentInput[currentInput.Length - 1];
        if (IsOperator(lastChar))
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
        }

        try
        {
            InputDisplay.Text = currentInput + " =";
            double result = EvaluateExpression(currentInput);
            ResultDisplay.Text = result.ToString(CultureInfo.InvariantCulture);
            currentInput = result.ToString(CultureInfo.InvariantCulture);
            isResultDisplayed = true;
        }
        catch (DivideByZeroException)
        {
            ResultDisplay.Text = "Error: Div by 0";
            currentInput = "";
            isResultDisplayed = true;
        }
        catch (Exception)
        {
            ResultDisplay.Text = "Error";
            currentInput = "";
            isResultDisplayed = true;
        }
    }

    private void UpdateDisplay()
    {
        ResultDisplay.Text = string.IsNullOrEmpty(currentInput) ? "0" : currentInput;
    }

    private bool IsOperator(char c)
    {
        return c == '+' || c == '−' || c == '×' || c == '÷';
    }

    // A simple math evaluator supporting BODMAS logic
    private double EvaluateExpression(string expression)
    {
        // Add spaces around operators for easy parsing
        expression = expression.Replace("+", " + ")
                               .Replace("−", " − ")
                               .Replace("×", " × ")
                               .Replace("÷", " ÷ ");

        // Remove extra spaces
        expression = Regex.Replace(expression, @"\s+", " ").Trim();

        string[] tokens = expression.Split(' ');
        var parsedTokens = new List<string>(tokens);

        // Pass 1: Handle Multiplication and Division (Higher precedence)
        for (int i = 0; i < parsedTokens.Count; i++)
        {
            if (parsedTokens[i] == "×" || parsedTokens[i] == "÷")
            {
                double left = double.Parse(parsedTokens[i - 1], CultureInfo.InvariantCulture);
                double right = double.Parse(parsedTokens[i + 1], CultureInfo.InvariantCulture);
                double result = 0;

                if (parsedTokens[i] == "×")
                {
                    result = left * right;
                }
                else if (parsedTokens[i] == "÷")
                {
                    if (right == 0) throw new DivideByZeroException();
                    result = left / right;
                }

                // Replace left, operator, and right with the computed value
                parsedTokens[i - 1] = result.ToString(CultureInfo.InvariantCulture);
                parsedTokens.RemoveRange(i, 2);
                i--; // Adjust index since we removed tokens
            }
        }

        // Pass 2: Handle Addition and Subtraction (Lower precedence)
        double finalResult = double.Parse(parsedTokens[0], CultureInfo.InvariantCulture);
        for (int i = 1; i < parsedTokens.Count; i += 2)
        {
            string op = parsedTokens[i];
            double right = double.Parse(parsedTokens[i + 1], CultureInfo.InvariantCulture);

            if (op == "+")
            {
                finalResult += right;
            }
            else if (op == "−")
            {
                finalResult -= right;
            }
        }

        return finalResult;
    }
}

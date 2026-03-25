namespace midterm;

public partial class MainPage : ContentPage
{
    private string _currentEntry = "0";
    private double _runningTotal = 0;
    private string _pendingOperator = "";
    private bool _isNewEntry = true;

    public MainPage()
    {
        InitializeComponent();
        UpdateDisplay();
    }

    private void OnDigitClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        string digit = button.Text;

        if (_isNewEntry)
        {
            _currentEntry = (digit == ".") ? "0." : digit;
            _isNewEntry = false;
        }
        else
        {
            if (digit == "." && _currentEntry.Contains("."))
                return;
            if (_currentEntry == "0" && digit != ".")
                _currentEntry = digit;
            else
                _currentEntry += digit;
        }

        UpdateDisplay();
    }

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        string op = button.Text;

        if (!_isNewEntry)
            CalculatePending();

        _pendingOperator = op;
        _isNewEntry = true;
    }

    private void OnCalculateClicked(object sender, EventArgs e)
    {
        CalculatePending();
        _pendingOperator = "";
        _isNewEntry = true;
    }

    private void CalculatePending()
    {
        if (!double.TryParse(_currentEntry, out double currentValue))
            return;

        if (string.IsNullOrEmpty(_pendingOperator))
        {
            _runningTotal = currentValue;
        }
        else
        {
            switch (_pendingOperator)
            {
                case "+": _runningTotal += currentValue; break;
                case "-": _runningTotal -= currentValue; break;
                case "x": _runningTotal *= currentValue; break;
                case "/":
                    if (currentValue == 0) { _currentEntry = "Error"; _isNewEntry = true; _pendingOperator = ""; UpdateDisplay(); return; }
                    _runningTotal /= currentValue;
                    break;
            }
        }

        _currentEntry = _runningTotal.ToString("G10");
        UpdateDisplay();
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        _currentEntry = "0";
        _runningTotal = 0;
        _pendingOperator = "";
        _isNewEntry = true;
        UpdateDisplay();
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_isNewEntry) return;

        if (_currentEntry.Length > 1)
        {
            _currentEntry = _currentEntry.Substring(0, _currentEntry.Length - 1);
            if (_currentEntry == "-") _currentEntry = "0";
        }
        else
        {
            _currentEntry = "0";
            _isNewEntry = true;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        DisplayLabel.Text = _currentEntry;
    }
}

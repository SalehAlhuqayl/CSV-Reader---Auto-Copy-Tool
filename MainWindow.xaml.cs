using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CsvReader.Services;
using CsvReader.Models;
using CsvReader.Utilities;

namespace CsvReader
{
    /// <summary>
    /// Main window for CSV Reader & Auto-Copy Tool
    /// </summary>
    public partial class MainWindow : Window
    {
        private CsvReaderService? _csvReaderService;
        private ExcelReaderService? _excelReaderService;
        private NavigationState? _navigationState;
        private CellNavigator? _cellNavigator;
        private ClipboardService? _clipboardService;
        private bool _isDarkMode = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle keyboard shortcuts
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_cellNavigator == null) return;

            switch (e.Key)
            {
                case Key.Right:
                case Key.Down:
                case Key.Space:
                    if (_cellNavigator.CanMoveNext())
                    {
                        NextButton_Click(this, new RoutedEventArgs());
                    }
                    e.Handled = true;
                    break;

                case Key.Left:
                case Key.Up:
                    if (_cellNavigator.CanMoveBack())
                    {
                        BackButton_Click(this, new RoutedEventArgs());
                    }
                    e.Handled = true;
                    break;

                case Key.F1:
                    HelpButton_Click(this, new RoutedEventArgs());
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Toggle dark/light theme
        /// </summary>
        private void ThemeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _isDarkMode = !_isDarkMode;

            if (_isDarkMode)
            {
                // Dark theme colors
                SetResourceColor("WindowBackground", "#1E1E1E");
                SetResourceColor("TextForeground", "#FFFFFF");
                SetResourceColor("HeaderForeground", "#B0B0B0");
                SetResourceColor("BorderBrush", "#404040");
                SetResourceColor("SubtleForeground", "#888888");
                ThemeToggleButton.Content = "☀️ Light";
            }
            else
            {
                // Light theme colors
                SetResourceColor("WindowBackground", "#FFFFFF");
                SetResourceColor("TextForeground", "#000000");
                SetResourceColor("HeaderForeground", "#555555");
                SetResourceColor("BorderBrush", "#CCCCCC");
                SetResourceColor("SubtleForeground", "#888888");
                ThemeToggleButton.Content = "🌙 Dark";
            }
        }

        /// <summary>
        /// Helper method to update resource colors
        /// </summary>
        private void SetResourceColor(string resourceKey, string hexColor)
        {
            var color = (Color)ColorConverter.ConvertFromString(hexColor);
            this.Resources[resourceKey] = new SolidColorBrush(color);
        }

        /// <summary>
        /// Show help dialog
        /// </summary>
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            var helpMessage = @"CSV Reader & Auto-Copy Tool - Help

📂 LOADING FILES:
• Click 'Browse File' to select a CSV or Excel (.xlsx) file
• Check 'First row is header' if your file has column names in row 1
• Excel files are fully supported!

⌨️ KEYBOARD SHORTCUTS:
• Right Arrow / Down Arrow / Space → Next cell
• Left Arrow / Up Arrow → Previous cell
• F1 → Show this help

🔄 NAVIGATION:
• Empty cells are automatically skipped
• Click 'Next' or 'Back' buttons to navigate
• Enter row number in 'Jump to Row' box and click 'Go'
• Current position shown in status bar

📋 CLIPBOARD:
• Each cell value is automatically copied to clipboard
• Click on displayed text to copy again manually
• Use Ctrl+V to paste copied value

🎨 THEME:
• Click '🌙 Dark' or '☀️ Light' to toggle theme

💡 TIPS:
• Header names appear above each cell value
• Use Jump to Row for quick navigation to specific rows
• Dark mode works instantly - try it!
• Navigation wraps across rows automatically

Need more help? Check the README.md file";

            MessageBox.Show(helpMessage, "Help - CSV Reader", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Handle Browse CSV button click
        /// </summary>
        private void BrowseCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "Select a File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadCsvFile(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Load and parse CSV file
        /// </summary>
        private void LoadCsvFile(string filePath)
        {
            try
            {
                _clipboardService = new ClipboardService();
                List<List<string>> csvData;
                
                // Determine file type and load accordingly
                if (filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    // Excel support using built-in .NET libraries!
                    _excelReaderService = new ExcelReaderService();
                    csvData = _excelReaderService.LoadExcel(filePath);
                }
                else
                {
                    _csvReaderService = new CsvReaderService();
                    csvData = _csvReaderService.LoadCsv(filePath);
                }
                
                bool hasHeader = HasHeaderCheckBox.IsChecked ?? true;
                
                _navigationState = new NavigationState(csvData, hasHeader);
                _cellNavigator = new CellNavigator(_navigationState);

                FilePathText.Text = filePath;
                
                // Move to first non-empty cell
                if (_cellNavigator.MoveToFirstNonEmptyCell())
                {
                    DisplayCurrentCell();
                    BackButton.IsEnabled = false;
                    NextButton.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("The CSV file appears to be empty or contains no data.", 
                                  "Empty File", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading CSV file: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Display current cell value and copy to clipboard
        /// </summary>
        private void DisplayCurrentCell()
        {
            if (_navigationState == null || _cellNavigator == null || _clipboardService == null)
                return;

            var currentValue = _navigationState.GetCurrentValue();
            var headerText = _navigationState.GetCurrentHeader();
            
            // Display header
            if (_navigationState.HasHeader)
            {
                HeaderLabel.Text = headerText;
                HeaderLabel.Visibility = Visibility.Visible;
            }
            else
            {
                HeaderLabel.Visibility = Visibility.Collapsed;
            }
            
            // Display cell value
            CurrentCellDisplay.Text = currentValue;

            // Auto-copy to clipboard
            _clipboardService.CopyToClipboard(currentValue);

            // Update status bar with header info
            var dataRowNumber = _navigationState.HasHeader ? _navigationState.CurrentRow : _navigationState.CurrentRow + 1;
            StatusText.Text = $"{headerText} | Row {dataRowNumber}, Column {_navigationState.CurrentColumn + 1} - Copied to clipboard";

            // Update button states
            BackButton.IsEnabled = _cellNavigator.CanMoveBack();
            NextButton.IsEnabled = _cellNavigator.CanMoveNext();
        }

        /// <summary>
        /// Handle Next button click
        /// </summary>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cellNavigator?.MoveNext() == true)
            {
                DisplayCurrentCell();
            }
        }

        /// <summary>
        /// Handle Back button click
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cellNavigator?.MoveBack() == true)
            {
                DisplayCurrentCell();
            }
        }

        /// <summary>
        /// Handle manual copy when user clicks on the cell display
        /// </summary>
        private void CurrentCellDisplay_Click(object sender, MouseButtonEventArgs e)
        {
            if (_navigationState == null || _clipboardService == null)
                return;

            var currentValue = _navigationState.GetCurrentValue();
            var headerText = _navigationState.GetCurrentHeader();
            _clipboardService.CopyToClipboard(currentValue);
            
            var dataRowNumber = _navigationState.HasHeader ? _navigationState.CurrentRow : _navigationState.CurrentRow + 1;
            StatusText.Text = $"{headerText} | Row {dataRowNumber}, Column {_navigationState.CurrentColumn + 1} - Manually copied";
        }

        /// <summary>
        /// Handle Jump to Row button click
        /// </summary>
        private void JumpToRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (_navigationState == null || _cellNavigator == null)
            {
                MessageBox.Show("Please load a file first.", "No File Loaded", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(JumpToRowTextBox.Text))
            {
                MessageBox.Show("Please enter a row number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(JumpToRowTextBox.Text, out int targetRow) || targetRow < 1)
            {
                MessageBox.Show("Please enter a valid row number (greater than 0).", "Invalid Row Number", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Adjust for header row if present
            int actualRow = _navigationState.HasHeader ? targetRow : targetRow - 1;

            if (actualRow >= _navigationState.GetRowCount())
            {
                MessageBox.Show($"Row {targetRow} does not exist. File has {_navigationState.GetRowCount() - (_navigationState.HasHeader ? 1 : 0)} data rows.", 
                              "Row Out of Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Jump to the target row, first column
            _navigationState.CurrentRow = actualRow;
            _navigationState.CurrentColumn = 0;

            // Find first non-empty cell in that row
            bool foundNonEmpty = false;
            int colCount = _navigationState.GetColumnCount(actualRow);
            
            for (int col = 0; col < colCount; col++)
            {
                if (!_navigationState.IsCellEmpty(actualRow, col))
                {
                    _navigationState.CurrentColumn = col;
                    foundNonEmpty = true;
                    break;
                }
            }

            if (!foundNonEmpty)
            {
                MessageBox.Show($"Row {targetRow} appears to be empty.", "Empty Row", MessageBoxButton.OK, MessageBoxImage.Information);
                // Stay at the row even if empty
            }

            DisplayCurrentCell();
            JumpToRowTextBox.Clear();
        }
    }
}

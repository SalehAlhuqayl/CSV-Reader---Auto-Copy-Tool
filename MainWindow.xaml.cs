using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
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
        private NavigationState? _navigationState;
        private CellNavigator? _cellNavigator;
        private ClipboardService? _clipboardService;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle Browse CSV button click
        /// </summary>
        private void BrowseCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Select a CSV File"
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
                _csvReaderService = new CsvReaderService();
                _clipboardService = new ClipboardService();
                
                var csvData = _csvReaderService.LoadCsv(filePath);
                
                _navigationState = new NavigationState(csvData);
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
            CurrentCellDisplay.Text = currentValue;

            // Auto-copy to clipboard
            _clipboardService.CopyToClipboard(currentValue);

            // Update status bar
            StatusText.Text = $"Row {_navigationState.CurrentRow + 1}, Column {_navigationState.CurrentColumn + 1} - Copied to clipboard";

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
            _clipboardService.CopyToClipboard(currentValue);
            
            StatusText.Text = $"Row {_navigationState.CurrentRow + 1}, Column {_navigationState.CurrentColumn + 1} - Manually copied to clipboard";
        }
    }
}

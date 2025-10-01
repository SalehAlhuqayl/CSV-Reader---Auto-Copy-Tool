using System.Collections.Generic;

namespace CsvReader.Models
{
    /// <summary>
    /// Represents the current navigation state in the CSV data
    /// </summary>
    public class NavigationState
    {
        public List<List<string>> CsvData { get; }
        public List<string>? HeaderRow { get; private set; }
        public bool HasHeader { get; private set; }
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }

        public NavigationState(List<List<string>> csvData, bool hasHeader = true)
        {
            CsvData = csvData;
            HasHeader = hasHeader;
            
            // Extract header row if present
            if (hasHeader && csvData.Count > 0)
            {
                HeaderRow = csvData[0];
                CurrentRow = 1; // Start from first data row
            }
            else
            {
                HeaderRow = null;
                CurrentRow = 0;
            }
            
            CurrentColumn = 0;
        }

        /// <summary>
        /// Get the value of the current cell
        /// </summary>
        public string GetCurrentValue()
        {
            if (CurrentRow < 0 || CurrentRow >= CsvData.Count)
                return string.Empty;

            var row = CsvData[CurrentRow];
            if (CurrentColumn < 0 || CurrentColumn >= row.Count)
                return string.Empty;

            return row[CurrentColumn];
        }

        /// <summary>
        /// Check if the current cell is empty
        /// </summary>
        public bool IsCurrentCellEmpty()
        {
            return string.IsNullOrWhiteSpace(GetCurrentValue());
        }

        /// <summary>
        /// Check if a specific cell is empty
        /// </summary>
        public bool IsCellEmpty(int row, int column)
        {
            if (row < 0 || row >= CsvData.Count)
                return true;

            var rowData = CsvData[row];
            if (column < 0 || column >= rowData.Count)
                return true;

            return string.IsNullOrWhiteSpace(rowData[column]);
        }

        /// <summary>
        /// Get the total number of rows
        /// </summary>
        public int GetRowCount()
        {
            return CsvData.Count;
        }

        /// <summary>
        /// Get the number of columns in a specific row
        /// </summary>
        public int GetColumnCount(int row)
        {
            if (row < 0 || row >= CsvData.Count)
                return 0;

            return CsvData[row].Count;
        }

        /// <summary>
        /// Get the header name for the current column
        /// </summary>
        public string GetCurrentHeader()
        {
            if (!HasHeader || HeaderRow == null)
                return $"Column {CurrentColumn + 1}";

            if (CurrentColumn < 0 || CurrentColumn >= HeaderRow.Count)
                return $"Column {CurrentColumn + 1}";

            var headerValue = HeaderRow[CurrentColumn];
            return string.IsNullOrWhiteSpace(headerValue) 
                ? $"Column {CurrentColumn + 1}" 
                : headerValue;
        }
    }
}

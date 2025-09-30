using System.Collections.Generic;
using CsvReader.Models;

namespace CsvReader.Utilities
{
    /// <summary>
    /// Utility class for navigating through CSV cells, skipping empty cells
    /// </summary>
    public class CellNavigator
    {
        private readonly NavigationState _state;
        private readonly List<(int Row, int Column)> _navigationHistory;

        public CellNavigator(NavigationState state)
        {
            _state = state;
            _navigationHistory = new List<(int Row, int Column)>();
        }

        /// <summary>
        /// Move to the first non-empty cell
        /// </summary>
        public bool MoveToFirstNonEmptyCell()
        {
            _state.CurrentRow = 0;
            _state.CurrentColumn = 0;

            // If current cell is not empty, we're done
            if (!_state.IsCurrentCellEmpty())
            {
                RecordPosition();
                return true;
            }

            // Otherwise, find the first non-empty cell
            return MoveNext();
        }

        /// <summary>
        /// Move to the next non-empty cell
        /// </summary>
        public bool MoveNext()
        {
            int startRow = _state.CurrentRow;
            int startColumn = _state.CurrentColumn;

            // Try to find next non-empty cell
            while (true)
            {
                // Move to next position
                _state.CurrentColumn++;

                // If we've gone past the current row, move to next row
                if (_state.CurrentColumn >= _state.GetColumnCount(_state.CurrentRow))
                {
                    _state.CurrentRow++;
                    _state.CurrentColumn = 0;
                }

                // Check if we've gone past all data
                if (_state.CurrentRow >= _state.GetRowCount())
                {
                    // Restore previous position
                    _state.CurrentRow = startRow;
                    _state.CurrentColumn = startColumn;
                    return false;
                }

                // Check if current cell is not empty
                if (!_state.IsCurrentCellEmpty())
                {
                    RecordPosition();
                    return true;
                }
            }
        }

        /// <summary>
        /// Move to the previous non-empty cell
        /// </summary>
        public bool MoveBack()
        {
            // Remove current position from history
            if (_navigationHistory.Count > 0)
            {
                _navigationHistory.RemoveAt(_navigationHistory.Count - 1);
            }

            // If there's a previous position, go there
            if (_navigationHistory.Count > 0)
            {
                var previousPosition = _navigationHistory[_navigationHistory.Count - 1];
                _state.CurrentRow = previousPosition.Row;
                _state.CurrentColumn = previousPosition.Column;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if we can move to the next cell
        /// </summary>
        public bool CanMoveNext()
        {
            // Save current state
            int currentRow = _state.CurrentRow;
            int currentColumn = _state.CurrentColumn;

            // Try to find next non-empty cell
            _state.CurrentColumn++;

            while (true)
            {
                // If we've gone past the current row, move to next row
                if (_state.CurrentColumn >= _state.GetColumnCount(_state.CurrentRow))
                {
                    _state.CurrentRow++;
                    _state.CurrentColumn = 0;
                }

                // Check if we've gone past all data
                if (_state.CurrentRow >= _state.GetRowCount())
                {
                    // Restore state
                    _state.CurrentRow = currentRow;
                    _state.CurrentColumn = currentColumn;
                    return false;
                }

                // Check if current cell is not empty
                if (!_state.IsCurrentCellEmpty())
                {
                    // Restore state
                    _state.CurrentRow = currentRow;
                    _state.CurrentColumn = currentColumn;
                    return true;
                }

                _state.CurrentColumn++;
            }
        }

        /// <summary>
        /// Check if we can move back
        /// </summary>
        public bool CanMoveBack()
        {
            return _navigationHistory.Count > 1;
        }

        /// <summary>
        /// Record current position in navigation history
        /// </summary>
        private void RecordPosition()
        {
            _navigationHistory.Add((_state.CurrentRow, _state.CurrentColumn));
        }
    }
}

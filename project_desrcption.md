# CSV Reader & Auto-Copy Tool (Windows Version)

## Overview
This project is a lightweight Windows desktop application built with **C# (WPF)**.  
Its purpose is to read a CSV file row by row, automatically copy cell values to the clipboard, and display them in a simple user interface.

---

## Core Features
1. **CSV Reading**
   - Load a CSV file from the local system.
   - Process data row by row.

2. **Cell Display & Auto-Copy**
   - Display each cell’s value in sequence.
   - Automatically copy the current cell’s value to the clipboard.
   - Skip empty cells completely (do not display or copy them).

3. **Navigation**
   - Automatically move through cells in order (left → right).
   - After finishing a row, move to the next row automatically.
   - Provide a **Back** button to return to previous cells.
   - Allow **manual copy** by clicking on a cell, even if it was auto-copied before.

4. **User Interface (WPF)**
   - Simple window with:
     - CSV File Upload button.
     - Current Cell Display area.
     - Navigation controls: **Next**, **Back**, **Jump to Row** (optional).
     - Status bar: current row/column position.

---

## Technical Requirements

### Language & Framework
- **C# .NET 6/7**
- **WPF (Windows Presentation Foundation)**

### Dependencies
- **CSV Helper** (NuGet package) → For parsing CSV files.
- **System.Windows.Clipboard** → For handling clipboard operations.

### Development Tools
- **Visual Studio 2022 (Community Edition or higher)**  
  - .NET Desktop Development workload enabled.

---

## Implementation Plan

### Step 1: Project Setup
- Create new WPF project in Visual Studio.
- Install `CsvHelper` NuGet package.

### Step 2: File Loading
- Implement a "Browse CSV" button.
- Use `CsvHelper` to read and store rows internally (e.g., `List<List<string>>`).

### Step 3: Navigation Logic
- Maintain state: `currentRow`, `currentColumn`.
- Implement `Next` and `Back` functions:
  - **Next**: move to the next non-empty cell.
  - **Back**: move to the previous non-empty cell.

### Step 4: Clipboard Integration
- On loading each cell:
  - Display content in UI.
  - Copy automatically to clipboard via `Clipboard.SetText(value)`.

### Step 5: Manual Copy Support
- Allow user to click any displayed cell → re-copy content to clipboard.

### Step 6: UI Enhancements
- Add status bar (e.g., "Row 3, Column 2").
- Add option to **Jump to specific row** (optional).

### Step 7: Testing
- Test with CSV files containing:
  - Full rows.
  - Empty cells.
  - Mixed data (numbers, text, special characters).
- Confirm skipping of empty cells works properly.

---

## Deliverables
- **Executable (.exe)** Windows desktop app.
- **Source code** in Visual Studio solution folder.
- **README file** with usage instructions.

---

## Future Enhancements (Optional)
- Support Excel `.xlsx` files.
- Add keyboard shortcuts for faster navigation.
- Provide dark/light theme toggle.
- Add error handling for corrupted CSVs.

# CSV Reader & Auto-Copy Tool

A simple Windows desktop app that reads CSV files and automatically copies cell values to your clipboard.

## What Does It Do?

This tool helps you work with CSV files by:
- Reading CSV files row by row
- Showing each cell's value on screen
- Automatically copying each cell to your clipboard
- Skipping empty cells so you don't waste time
- Letting you go back if you need to

## How to Use

1. **Open the app** - Double-click the CsvReader.exe file
2. **Load your CSV file** - Click "Browse CSV File" and choose your file
3. **Navigate through cells**:
   - Click "Next" to move to the next cell (it copies automatically!)
   - Click "Back" to go to the previous cell
   - Click on the displayed text to copy it again
4. **Paste wherever you need** - The current cell value is always on your clipboard

## Requirements

- Windows 10 or Windows 11
- .NET 7.0 Runtime (download from Microsoft if you don't have it)

## Building from Source

If you want to build the app yourself:

1. Install Visual Studio 2022 with .NET Desktop Development workload
2. Open the project folder
3. Run: `dotnet restore`
4. Run: `dotnet build`
5. Find the exe in `bin/Debug/net7.0-windows/`

Or build a release version:
```
dotnet publish -c Release -r win-x64 --self-contained
```

## Features

✅ Automatic clipboard copying  
✅ Empty cell skipping  
✅ Next/Back navigation  
✅ Manual re-copy by clicking  
✅ Position tracking (row & column display)  
✅ Simple, clean interface  

## Tips

- The app skips empty cells automatically, so you'll only see cells with data
- The status bar shows your current position (row and column number)
- You can click on the displayed text to copy it again without navigating
- The app reads all CSV formats - headers are treated as regular data

## Troubleshooting

**"Error loading CSV file"**  
- Make sure the file is a valid CSV
- Check that the file isn't open in another program

**Clipboard not working**  
- Another program might be using the clipboard
- Try clicking the cell display to copy manually

## Future Features

We're planning to add:
- Support for Excel files (.xlsx)
- Keyboard shortcuts for faster navigation
- Dark mode theme
- Remember your last file and position

## License

This project is open source. Feel free to use and modify it.

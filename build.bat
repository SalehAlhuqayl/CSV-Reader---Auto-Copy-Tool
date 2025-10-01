@echo off
echo Building CSV Reader with new features...
echo.
dotnet clean
dotnet build --no-restore
if %errorlevel% equ 0 (
    echo.
    echo ===================================
    echo BUILD SUCCESSFUL!
    echo ===================================
    echo.
    echo New features added:
    echo  [✓] Header Row Display
    echo  [✓] Dark Mode Theme  
    echo  [✓] Keyboard Shortcuts
    echo  [✓] Help System
    echo  [~] Excel Support (temporarily disabled - network issue)
    echo.
    echo Run the app with: dotnet run
    echo.
) else (
    echo.
    echo BUILD FAILED - See errors above
)
pause


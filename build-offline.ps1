# Build script that works offline using cached packages

Write-Host "Building CSV Reader (Offline Mode)..." -ForegroundColor Cyan
Write-Host ""

# Try to build without any network calls
$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"

# Option 1: Try build with no restore
Write-Host "Attempting build with cached packages..." -ForegroundColor Yellow
dotnet build --no-restore 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "=====================================" -ForegroundColor Green
    Write-Host "BUILD SUCCESSFUL!" -ForegroundColor Green  
    Write-Host "=====================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "New features added:" -ForegroundColor Cyan
    Write-Host "  [✓] Header Row Display" -ForegroundColor Green
    Write-Host "  [✓] Dark Mode Theme" -ForegroundColor Green
    Write-Host "  [✓] Keyboard Shortcuts" -ForegroundColor Green
    Write-Host "  [✓] Help System" -ForegroundColor Green
    Write-Host "  [~] Excel Support (temporarily disabled)" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Run the app with: dotnet run --no-restore" -ForegroundColor White
} else {
    Write-Host ""
    Write-Host "Build failed. Trying alternative method..." -ForegroundColor Yellow
    Write-Host ""
    
    # Option 2: Just run the existing build
    Write-Host "Running existing compiled version..." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Note: This runs the OLD version without new features." -ForegroundColor Red
    Write-Host "To get new features working, we need to resolve the network issue." -ForegroundColor Red
    Write-Host ""
    
    $exePath = "bin\Release\net9.0-windows\win-x64\publish\CsvReader.exe"
    if (Test-Path $exePath) {
        Write-Host "Starting old version from: $exePath" -ForegroundColor Cyan
        Start-Process $exePath
    } else {
        Write-Host "No compiled version found. Need to fix network issue to build." -ForegroundColor Red
    }
}


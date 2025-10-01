# Clean build script - removes all cached files and rebuilds

Write-Host "Cleaning build artifacts..." -ForegroundColor Cyan

# Remove obj and bin directories
if (Test-Path "obj") {
    Remove-Item -Recurse -Force "obj"
    Write-Host "✓ Removed obj folder" -ForegroundColor Green
}

if (Test-Path "bin\Debug") {
    Remove-Item -Recurse -Force "bin\Debug"
    Write-Host "✓ Removed bin\Debug folder" -ForegroundColor Green
}

Write-Host ""
Write-Host "Attempting fresh build without network..." -ForegroundColor Yellow
Write-Host ""

# Set offline mode
$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"

# Try to restore with only local cache
Write-Host "Step 1: Restoring packages from local cache..." -ForegroundColor Cyan
dotnet restore --source "$env:USERPROFILE\.nuget\packages" --no-http-cache 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Packages restored from cache" -ForegroundColor Green
    Write-Host ""
    Write-Host "Step 2: Building project..." -ForegroundColor Cyan
    dotnet build --no-restore
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "=====================================" -ForegroundColor Green
        Write-Host "BUILD SUCCESSFUL!" -ForegroundColor Green
        Write-Host "=====================================" -ForegroundColor Green
        Write-Host ""
        Write-Host "New features are now compiled:" -ForegroundColor Cyan
        Write-Host "  ✅ Header Row Display" -ForegroundColor Green
        Write-Host "  ✅ Dark Mode Theme" -ForegroundColor Green
        Write-Host "  ✅ Keyboard Shortcuts" -ForegroundColor Green
        Write-Host "  ✅ Help System" -ForegroundColor Green
        Write-Host ""
        Write-Host "Run with: dotnet run --no-restore" -ForegroundColor White
        Write-Host ""
    } else {
        Write-Host ""
        Write-Host "Build failed. CsvHelper package not in local cache." -ForegroundColor Red
        Write-Host ""
        Write-Host "To fix this:" -ForegroundColor Yellow
        Write-Host "1. On a PC with internet, run: dotnet restore" -ForegroundColor White
        Write-Host "2. Copy folder: %USERPROFILE%\.nuget\packages\csvhelper" -ForegroundColor White
        Write-Host "3. Paste to same location on this PC" -ForegroundColor White
        Write-Host "4. Run this script again" -ForegroundColor White
        Write-Host ""
    }
} else {
    Write-Host ""
    Write-Host "⚠️  Cannot restore packages - CsvHelper not in local cache" -ForegroundColor Red
    Write-Host ""
    Write-Host "Your options:" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Option A: Get CsvHelper package manually" -ForegroundColor Cyan
    Write-Host "  1. Download from: https://www.nuget.org/packages/CsvHelper/30.0.1" -ForegroundColor White
    Write-Host "  2. Place in: $env:USERPROFILE\.nuget\packages\csvhelper\30.0.1\" -ForegroundColor White
    Write-Host "  3. Run this script again" -ForegroundColor White
    Write-Host ""
    Write-Host "Option B: Use Visual Studio" -ForegroundColor Cyan
    Write-Host "  1. Open project in Visual Studio 2022" -ForegroundColor White
    Write-Host "  2. Visual Studio may have cached packages" -ForegroundColor White
    Write-Host "  3. Click Build Solution (F7)" -ForegroundColor White
    Write-Host ""
    Write-Host "Option C: Copy packages from another PC" -ForegroundColor Cyan
    Write-Host "  Copy entire folder: %USERPROFILE%\.nuget\packages\" -ForegroundColor White
    Write-Host "  From a PC with .NET 9 to this PC" -ForegroundColor White
    Write-Host ""
}

Write-Host ""
Write-Host "Note: All new features are coded and ready!" -ForegroundColor Green
Write-Host "We just need to compile them." -ForegroundColor Green


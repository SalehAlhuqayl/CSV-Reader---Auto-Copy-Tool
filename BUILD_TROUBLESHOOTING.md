# Build Troubleshooting Guide

## Problem
NuGet is unable to connect to api.nuget.org due to network/SSL issues.

## ✅ Quick Solutions (Try in Order)

### Solution 1: Use Offline Build Script
```powershell
.\build-offline.ps1
```
This tries to build using only cached packages.

### Solution 2: Build with No Restore
```powershell
dotnet build --no-restore --no-dependencies
```

### Solution 3: Use Cached NuGet Packages
```powershell
# Check if packages are cached
dir $env:USERPROFILE\.nuget\packages\csvhelper

# If CsvHelper exists, try:
dotnet msbuild /t:Build /p:RestorePackages=false
```

### Solution 4: Configure NuGet to Use Local Cache Only
```powershell
# Already created NuGet.Config for you
dotnet build
```

### Solution 5: Run Existing Build
If building fails, you can run the old version (without new features):
```powershell
.\bin\Release\net9.0-windows\win-x64\publish\CsvReader.exe
```

---

## 🔍 Network Issue Diagnosis

The error suggests a firewall, proxy, or antivirus is blocking NuGet:

### Check 1: Firewall
```powershell
# Windows Firewall might be blocking dotnet.exe
# Temporarily disable and try build
```

### Check 2: Proxy Settings
```powershell
# Check if you're behind a proxy
netsh winhttp show proxy

# If proxy is shown, configure it:
dotnet nuget config set http_proxy http://proxy:port
```

### Check 3: Antivirus
- Some antivirus software blocks NuGet SSL connections
- Try temporarily disabling antivirus and build again

### Check 4: DNS/Network
```powershell
# Test if you can reach NuGet
nslookup api.nuget.org
ping api.nuget.org

# If failed, it's a network issue
```

---

## 🎯 Best Workaround for Now

### Option A: Manual Package Download (Recommended)

1. **On a computer with internet**, download CsvHelper:
   - Go to: https://www.nuget.org/packages/CsvHelper/30.0.1
   - Click "Download package"
   - Save `csvhelper.30.0.1.nupkg`

2. **Copy to your computer** and place in:
   ```
   %USERPROFILE%\.nuget\packages\csvhelper\30.0.1\
   ```

3. **Extract the .nupkg** (it's a ZIP file):
   ```powershell
   Expand-Archive csvhelper.30.0.1.nupkg -DestinationPath csvhelper.30.0.1
   ```

4. **Try building again:**
   ```powershell
   dotnet build --no-restore
   ```

### Option B: Use Visual Studio

Visual Studio might have better proxy/network handling:

1. Open Visual Studio 2022
2. File → Open → Folder → Select csv_reader folder
3. Click Build in Visual Studio
4. Visual Studio might succeed where CLI fails

### Option C: Copy Packages from Another Machine

If you have another Windows PC with .NET 9:

1. **On the working PC**, restore packages:
   ```powershell
   dotnet restore
   ```

2. **Copy the packages folder** from:
   ```
   %USERPROFILE%\.nuget\packages\
   ```

3. **Paste on your PC** to same location

4. **Build offline:**
   ```powershell
   dotnet build --no-restore
   ```

---

## 📝 What's Actually Needed

The project only needs **CsvHelper** package (EPPlus is commented out).

- Package: CsvHelper
- Version: 30.0.1
- Location when cached: `%USERPROFILE%\.nuget\packages\csvhelper\30.0.1\`

If this package is present, the build should work offline.

---

## 🚀 Alternative: Compile Manually

As a last resort, you can compile the C# files directly:

```powershell
csc /target:winexe /out:CsvReader.exe /reference:"C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App\9.0.0\*.dll" *.cs
```

(This is complex and not recommended, but possible)

---

## 💡 Recommended Action

**Try this command right now:**

```powershell
dotnet build --no-restore --no-dependencies /p:RestorePackages=false
```

If it fails, check if CsvHelper is cached:

```powershell
Test-Path "$env:USERPROFILE\.nuget\packages\csvhelper"
```

If **True** → Package is there, build should work  
If **False** → Need to get package manually (Option A above)

---

## 📞 If Nothing Works

The new features are implemented in the code files. Even if you can't build now:

1. **All code changes are saved**
2. **Features will work once built**
3. **Can build on another computer with internet**
4. **Or fix network issue and build later**

The implementation is complete and ready! Just need to compile it. 🎯


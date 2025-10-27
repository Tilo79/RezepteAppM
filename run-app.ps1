# RezepteApp - Automatischer Clean Build & Run
# Dieses Skript stoppt alte Instanzen, loescht die DB und startet die App neu

Write-Host "Stoppe laufende RezepteApp-Instanzen..." -ForegroundColor Yellow
Get-Process -Name "RezepteApp" -ErrorAction SilentlyContinue | Stop-Process -Force
Start-Sleep -Seconds 2

Write-Host "Loesche alte Datenbank..." -ForegroundColor Yellow
$dbPath = Join-Path $env:LOCALAPPDATA "RezepteApp\recipes.db3"
if (Test-Path $dbPath) {
    Remove-Item $dbPath -Force
    Write-Host "   Datenbank geloescht: $dbPath" -ForegroundColor Green
} else {
    Write-Host "   Keine alte Datenbank gefunden" -ForegroundColor Gray
}

Write-Host "Clean Build..." -ForegroundColor Yellow
Set-Location "C:\Users\TiloS\RezepteAppM\RezepteApp"
dotnet build -t:Clean -f net9.0-windows10.0.19041.0 | Out-Null

Write-Host "Baue App..." -ForegroundColor Yellow
dotnet build -t:Restore,Build -f net9.0-windows10.0.19041.0

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build erfolgreich! Starte App..." -ForegroundColor Green
    Write-Host ""
    Start-Process -FilePath "C:\Users\TiloS\RezepteAppM\RezepteApp\bin\Debug\net9.0-windows10.0.19041.0\win10-x64\RezepteApp.exe"
    Write-Host "App wurde gestartet!" -ForegroundColor Green
} else {
    Write-Host "Build fehlgeschlagen!" -ForegroundColor Red
    exit 1
}

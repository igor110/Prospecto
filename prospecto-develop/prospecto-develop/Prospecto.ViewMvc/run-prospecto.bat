@echo off
echo Iniciando aplicação na porta 5000...
cd /d "%~dp0" 
dotnet run --urls "http://0.0.0.0:5000"
pause

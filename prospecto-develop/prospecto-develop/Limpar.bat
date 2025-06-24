@echo off
setlocal
title Limpeza de bin e obj

echo.
echo Limpando pastas bin e obj...

for /d /r %%i in (bin,obj) do (
    if exist "%%i" (
        echo Deletando %%i
        rmdir /s /q "%%i"
    )
)

echo.
echo Limpeza concluída!
pause
endlocal

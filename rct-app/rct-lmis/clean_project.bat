@echo off
echo Cleaning project folders...
for /d /r "." %%d in (bin,obj) do if exist "%%d" rd /s /q "%%d"
echo Done cleaning bin and obj folders!
pause

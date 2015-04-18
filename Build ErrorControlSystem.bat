@echo off
mkdir out

IF EXIST "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" (
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" "src\ErrorControlSystem.sln" /t:Rebuild /property:Configuration=Release /verbosity:normal
) ELSE (
"C:\Program Files\MSBuild\12.0\Bin\MSBuild.exe" "src\ErrorControlSystem.sln" /t:Rebuild /property:Configuration=Release /verbosity:normal
)

pause
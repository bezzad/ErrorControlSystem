@echo off
mkdir ..\..\out

IF EXIST "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" (
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" "..\ErrorControlSystem.sln" /t:Rebuild /property:Configuration=Release /verbosity:minimal
) ELSE (
"C:\Program Files\MSBuild\12.0\Bin\MSBuild.exe" "..\ErrorControlSystem.sln" /t:Rebuild /property:Configuration=Release /verbosity:minimal
)

mkdir ..\..\NuGetPackages 2> NUL
nuget pack -OutputDirectory ..\..\NuGetPackages ErrorControlSystem.nuspec
pause
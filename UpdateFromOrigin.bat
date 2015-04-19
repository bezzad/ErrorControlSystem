@echo off
Echo Start Updating ...

git checkout master
git fetch
git status
git pull

Echo Update Completed.
pause

Echo Start Building Project ...

mkdir out

IF EXIST "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" (
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" "src\ErrorControlSystem.sln" /t:clean /t:Rebuild /property:Configuration=Release /verbosity:minimal
) ELSE (
"C:\Program Files\MSBuild\12.0\Bin\MSBuild.exe" "src\ErrorControlSystem.sln" /t:clean /t:Rebuild /property:Configuration=Release /verbosity:minimal
)

Echo Build Completed.
pause
del build.log
mkdir out

msbuild ErrorControlSystem.sln /t:Clean /t:Rebuild /property:Configuration=Release /out build.log -o \out
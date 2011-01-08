mkdir nuget-package
MSBuild .\src\Options.sln "/T:Clean;Build" /P:Configuration=Release
.\tools\nuget.exe p .\src\Options.nuspec -b .\src\Options\bin\Release -o .\nuget-package
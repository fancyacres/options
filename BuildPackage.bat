mkdir nuget-package
MSBuild .\src\Options.sln "/T:Clean;Build" "/P:Configuration=Release;OutputPath=.\bin\Package\lib"
.\tools\nuget.exe p .\src\Options.nuspec -b .\src\Options\bin\Package -o .\nuget-package
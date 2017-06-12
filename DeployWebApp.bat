msbuild.exe .\DotNetUnitTests.sln /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=LocalWeb
start "" http://localhost:8080
call "C:\Program Files\IIS Express\iisexpress" /path:"%cd%\DotNetUnitTests" /port:8080
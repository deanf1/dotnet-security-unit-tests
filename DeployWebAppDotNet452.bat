IF EXIST "%cd%\DotNetUnitTests\Web.config" (
	DEL "%cd%\DotNetUnitTests\Web.config"
	)
COPY "%cd%\DotNetUnitTests\webconfigs\Web.452.config" "%cd%\DotNetUnitTests"
REN "%cd%\DotNetUnitTests\Web.452.config" "Web.config"

msbuild.exe .\DotNetUnitTests.sln /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=LocalWeb
START "" http://localhost:8080
CALL "C:\Program Files\IIS Express\iisexpress" /path:"%cd%\DotNetUnitTests" /port:8080
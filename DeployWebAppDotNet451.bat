IF EXIST "%cd%\DotNetUnitTests\Web.config" (
	DEL "%cd%\DotNetUnitTests\Web.config"
	)
COPY "%cd%\DotNetUnitTests\webconfigs\Web.451.config" "%cd%\DotNetUnitTests"
REN "%cd%\DotNetUnitTests\Web.451.config" "Web.config"

msbuild /m .\DotNetUnitTests.sln /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=LocalWeb
START "" http://localhost:8080
START /d "C:\Program Files\IIS Express\" iisexpress.exe /path:"%cd%\DotNetUnitTests" /port:8080

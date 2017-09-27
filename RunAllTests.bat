@echo off

pip install -U selenium

if exist .\chromedriver.exe (
	set PATH=%PATH%;%cd%\chromedriver.exe
)

python WebTestCrawler.py

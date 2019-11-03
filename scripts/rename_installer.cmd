@setlocal enabledelayedexpansion
@echo off

if [%APPVEYOR_BUILD_VERSION%] == [] goto :NO_VERSION

for /f %%i in ('git rev-parse --abbrev-ref HEAD') do set BRANCH_NAME=%%i
set BRANCH_NAME=%BRANCH_NAME:/=_%
set BRANCH_NAME=%BRANCH_NAME:\=_%
for /f %%i in ('git rev-parse --short HEAD') do set COMMIT_HASH=%%i

SET source_installer_name="Tailviewer-setup.exe"

if [%BRANCH_NAME%] == [master] (
	SET dest_installer_name="Tailviewer-setup-%APPVEYOR_BUILD_VERSION%.exe"
) else (
	SET dest_installer_name="Tailviewer-setup-branch-%BRANCH_NAME%-%COMMIT_HASH%.exe"
)

echo Renaming installer to %dest_installer_name%
ren %source_installer_name% %dest_installer_name% || goto :MOVE_ERROR
exit /b 0

:NO_VERSION
echo ERROR: Missing environment variable APPVEYOR_BUILD_VERSION!
exit /b -1

:MOVE_ERROR
echo ERROR: Unable to rename installer, ren returned %errorlevel%
exit /b -2

endlocal
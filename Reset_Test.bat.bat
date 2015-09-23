:: /Q = Quiet Mode, Do not prompt
del c:\DEV_SOURCE\*.* /Q
del c:\DEV_TARGET\*.* /Q

:: /Y = Overwrites existing files without prompt
xcopy C:\DEV_TESTDATA\Pictures  C:\DEV_SOURCE\*.* /Y

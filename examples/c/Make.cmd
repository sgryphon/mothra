REM Run in Visual Studio x64 Native Tools Command Prompt

set ROOT_DIR=%~dp0..\..
set BIND_DIR=%ROOT_DIR%\bindings
set CBIND_DIR=%BIND_DIR%\c
set OUT_DIR=%ROOT_DIR%\bin

set CFLAGS=/W4 /O2 /EHsc /MD
set INCLUDES=-I%CBIND_DIR%
set LFLAGS=/LIBPATH:"%OUT_DIR%" /LIBPATH:"%OUT_DIR%\release" /LIBPATH:"%OUT_DIR%\release\deps"
set OBJ=%OUT_DIR%\example.obj
set TARGET=%OUT_DIR%\example.exe

if not exist "%OUT_DIR%" ( mkdir "%OUT_DIR%" )
if exist "%TARGET%" ( del "%TARGET%" )
cl /c %CFLAGS% "example.c" %INCLUDES% /Fo"%OBJ%"
link mothra.dll.lib "%OBJ%" %LFLAGS% /OUT:"%TARGET%"

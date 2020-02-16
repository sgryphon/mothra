REM Run in Visual Studio x64 Native Tools Command Prompt

set EXT=dll
set ROOT_DIR=%~dp0..\..
set OUT_DIR=%ROOT_DIR%\bin
set CORE_DIR=%ROOT_DIR%\core
set BIND_DIR=%ROOT_DIR%\bindings
set CBIND_DIR=%BIND_DIR%\c

set CFLAGS=/W4 /O2 /EHsc /MD
set LFLAGS=
set INCLUDES=
set OBJ=%OUT_DIR%\mothra-ingress.obj
set TARGET=%OUT_DIR%\mothra-ingress.lib

if not exist "%OUT_DIR%" ( mkdir "%OUT_DIR%" )
if exist "%TARGET%" ( del "%TARGET%" )
cl /c /LD %CFLAGS% "%CBIND_DIR%/mothra-ingress.c" /Fo"%OBJ%"
lib "%OBJ%" /out:"%TARGET%"

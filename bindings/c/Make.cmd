REM Run in Visual Studio x64 Native Tools Command Prompt

set EXT=dll
set ROOT_DIR=%~dp0/../..
set OUT_DIR=%ROOT_DIR%/bin
set CORE_DIR=%ROOT_DIR%/core
set BIND_DIR=%ROOT_DIR%/bindings
set CBIND_DIR=%BIND_DIR%/c

set CFLAGS=/W4 /O2 /EHsc
set LFLAGS=
set INCLUDES=
set OBJ=%OUT_DIR%/mothra-ingress.obj
set TARGET=%OUT_DIR%/mothra-ingress.%EXT%

if not exist "%OUT_DIR%" ( mkdir "%OUT_DIR%" )
if exist "%TARGET%" ( del "%TARGET%" )
cl /LD %CFLAGS% "%CBIND_DIR%/mothra.c" /Fe"%TARGET%" /Fo"%OBJ%"

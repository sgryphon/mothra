REM Run in Visual Studio x64 Native Tools Command Prompt

set ROOT_DIR=%~dp0..\..
set OUT_DIR=%ROOT_DIR%\bin
set BIND_DIR=%ROOT_DIR%\bindings
set CBIND_DIR=%BIND_DIR%\c

set CFLAGS=/W4 /O2 /EHsc /MD
set LFLAGS=
set OBJ=%OUT_DIR%\mothra-ingress.obj
set LIB=%OUT_DIR%\mothra-ingress.lib

if not exist "%OUT_DIR%" ( mkdir "%OUT_DIR%" )

@echo ## Cleaning "%LIB%"
if exist "%LIB%" ( del "%LIB%" )

@echo ## Compiling "%OBJ%"
cl /c /LD %CFLAGS% "%CBIND_DIR%/mothra-ingress.c" /Fo"%OBJ%"

@echo ## Creating static ingress library "%LIB%"
lib %LFLAGS% "%OBJ%" /out:"%LIB%"

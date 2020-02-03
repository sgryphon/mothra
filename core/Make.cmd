REM Run in Visual Studio x64 Native Tools Command Prompt
REM Needs OpenSSL (use: vcpkg install openssl:x64-windows)
REM Needs Rust (also has a dependency on Visual Studio)

set EXT=dll
set ROOT_DIR=%~dp0..
set OUT_DIR=%ROOT_DIR%\bin
set OPENSSL_DIR=C:\code\vcpkg\packages\openssl-windows_x64-windows

rmdir /S /Q "%OUT_DIR%\release"

cargo build --release --target-dir="%OUT_DIR%"
if errorlevel 0 goto :copy
goto :end

:copy
robocopy "%OUT_DIR%\release" "%OUT_DIR%" "mothra.%EXT%"

:end
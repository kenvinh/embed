@echo off

set VIMSRC=C:\Users\KENVINH\Desktop\SourceGit\Collection\vim\src
REM PATH = "C:\Program Files (x86)\mingw-w64\i686-5.3.0-posix-dwarf-rt_v4-rev0\mingw32\bin"

set WORKDIR=%~dp0
set LOGFILE=%WORKDIR%log.txt

set SDK_INCLUDE_DIR=C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Include
SET VC_DIR="C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC

echo Set up compiler
CALL %VC_DIR%\vcvarsall.bat" x86_amd64

SET CPUNR=i686
SET WINVER=0x500
SET CPU=AMD64
SET NETBEANS=no
SET FEATURES=HUGE
SET OPTIMIZE=MAXSPEED

echo Work directory: %WORKDIR%
echo Vim source directory: %VIMSRC%
echo Visual C Studio directory: %VC_DIR%
echo SDK Include directory: %SDK_INCLUDE_DIR%

cd /d %VIMSRC%

REM --- Build GUI version (gvim.exe) ---
echo Building gvim.exe
%VC_DIR%\bin\nmake" -f Make_mvc.mak PYTHON="C:/Python27" PYTHON_VER=27 DYNAMIC_PYTHON=yes PYTHON3="C:\Users\KENVINH\AppData\Local\Programs\Python\Python35-32" PYTHON3_VER=35 DYNAMIC_PYTHON3=yes GUI=yes gvim.exe > "%LOGFILE%"

REM --- Build console version (vim.exe) ---
echo Building vim.exe ...
REM The following command will compile with both Python 2.7 and Python 3.3
%VC_DIR%\bin\nmake" -f Make_mvc.mak PYTHON="C:/Python27" PYTHON_VER=27 DYNAMIC_PYTHON=yes PYTHON3="C:\Users\KENVINH\AppData\Local\Programs\Python\Python35-32" PYTHON3_VER=35 DYNAMIC_PYTHON3=yes GUI=no vim.exe >> "%LOGFILE%"


echo Moving files ...
move gvim.exe "%WORKDIR%"
move vim.exe "%WORKDIR%"

cd /d %WORKDIR%

echo Cleaning Vim source directory ...
REM NOTE: "mingw32-make.exe -f Make_ming.mak clean" does not finish the job
IF NOT %CD%==%VIMSRC% GOTO THEEND
IF NOT EXIST vim.h GOTO THEEND
IF EXIST pathdef.c DEL pathdef.c
IF EXIST obj\NUL      RMDIR /S /Q obj
IF EXIST obji386\NUL  RMDIR /S /Q obji386
IF EXIST objx86-64\NUL  RMDIR /S /Q objx86-64
IF EXIST gobj\NUL     RMDIR /S /Q gobj
IF EXIST gobji386\NUL RMDIR /S /Q gobji386
IF EXIST gobjx86-64\NUL RMDIR /S /Q gobjx86-64
IF EXIST gvim.exe DEL gvim.exe
IF EXIST vim.exe  DEL vim.exe

:THEEND

pause
@echo off

SET DESTDIR=C:\Users\KENVINH\vim
SET VIMSRC=%DESTDIR%\build_src\vim\
SET BUILD_DIR=%DESTDIR%\build_src\batch\

SET SDK_INCLUDE_DIR=C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Include
SET VC_DIR="C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC

SET LOGFILE=%DESTDIR%log.txt

echo Set up compiler
CALL %VC_DIR%\vcvarsall.bat" x86_amd64

REM --- Set up build parameter ---
SET CPUNR=i686
SET WINVER=0x500
SET CPU=AMD64
SET NETBEANS=no
SET FEATURES=HUGE
SET OPTIMIZE=MAXSPEED
SET OLE=YES

REM --- Recheck directory ---
echo Work directory: %DESTDIR%
echo Vim source directory: %VIMSRC%
echo Batch file directory: %BUILD_DIR%
echo Visual C Studio directory: %VC_DIR%
echo SDK Include directory: %SDK_INCLUDE_DIR%

cd /d %VIMSRC%\src

REM --- Build GUI version (gvim.exe) ---
echo Building gvim.exe
%VC_DIR%\bin\nmake" -f Make_mvc.mak PYTHON="C:/Python27" PYTHON_VER=27 DYNAMIC_PYTHON=yes PYTHON3="C:\Users\KENVINH\AppData\Local\Programs\Python\Python35-32" PYTHON3_VER=35 DYNAMIC_PYTHON3=yes GUI=yes > "%LOGFILE%"

REM --- Build console version (vim.exe) ---
echo Building vim.exe ...
REM The following command will compile with both Python 2.7 and Python 3.3
%VC_DIR%\bin\nmake" -f Make_mvc.mak PYTHON="C:/Python27" PYTHON_VER=27 DYNAMIC_PYTHON=yes PYTHON3="C:\Users\KENVINH\AppData\Local\Programs\Python\Python35-32" PYTHON3_VER=35 DYNAMIC_PYTHON3=yes GUI=no >> "%LOGFILE%"
REM Dummy build to release vim.exe
%VC_DIR%\bin\nmake" -c

REM --- After build, move files and clean up source folder
REM ------ Move: all exe files to vim\
REM ------ Move: gvimext.dll file to vim\
REM ------ Copy: runtimes file to vim\runtimes\
REM ------ Cleanup: *.obj, object directory
REM ------ Cleanup: *.pdb
REM ------ Cleanup: pathdef.c
REM ------ Cleanup: GvimExt\gvimext.exp, gvimext.lib, gvimext.res

echo Moving files ...
CALL %BUILD_DIR%\flat_copy_overwrite.cmd "*.exe" "%VIMSRC%\src" "%DESTDIR%" /mov
MOVE /Y %VIMSRC%\src\GvimExt\gvimext.dll %DESTDIR%
ROBOCOPY %VIMSRC%\runtime\ %DESTDIR%\runtime\ /e /is /ns /nc /nfl /ndl /njh

echo Cleaning Vim built object files ...
FOR /R %VIMSRC% %%G IN (*.obj) do DEL "%%G"
IF EXIST ObjCYHAMD64	RMDIR /S /Q ObjCYHAMD64
IF EXIST ObjGYHAMD64	RMDIR /S /Q ObjGYHAMD64

echo Cleaning Vim built debug files ...
FOR /R %VIMSRC% %%G IN (*.pdb) do DEL "%%G"

echo Cleaning Vim source directory ...
IF EXIST pathdef.c DEL pathdef.c
IF EXIST GvimExt\gvimext.exp DEL GvimExt\gvimext.exp
IF EXIST GvimExt\gvimext.lib DEL GvimExt\gvimext.lib
IF EXIST GvimExt\gvimext.res DEL GvimExt\gvimext.res
IF EXIST vim.exe DEL vim.exe

cd /d %DESTDIR%

:THEEND

pause

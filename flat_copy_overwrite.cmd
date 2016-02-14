@echo off
SETLOCAL EnableDelayedExpansion

REM Function:		flat_copy_overwrite
REM Description:	Flat move/copy all recursively all file with desired extension to one single folder
REM					This will move/copy with overwriting option
REM					Check input argument exist or not
REM					Check path exist or not
REM					Check extension format
REM Input Argument:	%1: Extension format
REM					%2: Folder path to recursively search for file match extension
REM					%3: Folder path to move/copy to
REM					%4: Move or copy

set num_of_paths=0
set /a is_params_OK=1
set /a is_param4_exist=1
set NL=^


REM Must remain two empty line below NL

REM Check parameter existence
:: Param4
if [%4] == [] (
	set /a is_params_OK=0
	set err_msg= Must provide 4 inputs !NL!
	goto :error_message
)

REM Parse input parameter
	set extension=%~1
	set root_place=%2
	set dest=%3
	set cmd=%~4
	
	REM Check extension format
	if not [%extension:~0,2%]==[*.] (
		set /a is_params_OK=0
		set err_msg=!err_msg! Extension format not correct !NL!
	)
	
	REM Check path existence
	if not exist %root_place% (
		set /a is_params_OK=0
		set err_msg=!err_msg! Source path not exist !NL!
	)
	if not exist %dest% (
		set /a is_params_OK=0
		set err_msg=!err_msg! Destination path not exist !NL!
	)

	REM Check command
	if /i not [%cmd%]==[/mov] if /i not [%cmd%]==[/copy] (
		set /a is_params_OK=0
		set err_msg=!err_msg! Command accept "/mov" or "/copy" only !NL!
	)
	
	if /i [%cmd%]==[/copy] (
		set cmd=/copy:dat
	)
	REM PARAMETER CHECK DONE, perform 
	if %is_params_OK% EQU 1 (
		set i_var[0]=!root_place!
		FOR /F "tokens=*" %%G in ('dir /b /s /a:d  !root_place!\*') DO (
			set /a num_of_paths+=1
			set i_var[!num_of_paths!]="!i_var!%%G"
		)
		
		REM Loop through the folders and sub-folders to execute action
		echo Path:
		FOR /L %%i in (0, 1, !num_of_paths!) do (
			set /a j=%%i
			for %%J in (!j!) do (
				REM PUT CUSTOM ACTION HERE
				echo !i_var[%%J]!
				ROBOCOPY !i_var[%%J]! !dest! !extension! !cmd! /is /ns /nc /nfl /ndl /njh /njs
			)
		)
		
		set err_msg=!err_msg! FINISH [%~4] files
	)

:error_message
	echo !err_msg!

ENDLOCAL
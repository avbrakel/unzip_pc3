:: By SteeneK, 2020-01-16, rev.1

@ECHO off
Title EDIT PC3 - Instruction window.
If %1.==. goto SYNTAX

REM Enter your editor on next line...
set ED="C:\Program Files (x86)\Notepad++\notepad++.exe"
SET ASCII-EDITOR=START ""
If exist %ED% SET ASCII-EDITOR=%ED%

SET BKUP=%~dp1[%~n1].pc3

Copy /Y %1 %BKUP%

CLS
unzip_pc3 e %1
CLS
ECHO.
ECHO  Data has been extracted from PC3 file %~nx1 
ECHO  And the DATA-File %~nx1.txt will now have opened, for editing.
ECHO.
%ASCII-EDITOR% %1.txt
ECHO.
ECHO  If all went well, then there is also a Backup = [%~n1].pc3
ECHO  And next, %~nx1 wil open - with your changes included.
ECHO.
ECHO  But, first save your changes to the DATA-File.
ECHO  And then come back here, after you have saved the DATA-File,
ECHO  to press any key and continue - opening the updated %~nx1 . . .

Pause > Nul


cls
ECHO.
unzip_pc3 c %1.txt
CLS
ECHO.
ECHO  Save the PC3 file that will open next.
REM ECHO  Its PMP-file [if there is one], will then update too.

if not exist %BKUP% goto END
DEL %1 
REN %1.txt.pc3 %~nx1
start "" %1

ECHO.
ECHO  If you SAVE %~nx1, then its PMP file (if there is one) will update too
ECHO.
ECHO  After saving the PC3, do not forget to close this window . . .
Pause > Nul

goto END

:SYNTAX
ECHO.
ECHO  You must drag-drop a PC3 file on %0

ECHO  Press any key to to close this window . . .
Pause > Nul


:END
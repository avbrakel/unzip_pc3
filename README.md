unzip_pc3
=========

Compress (.TXT=>.PC3) and decompress (extract  .PC3/.PMP/.=>.TXT) pc3 files for AutoCAD 

.Exe can be found in unzip_pc3\bin\Debug\unzip_pc3.exe
it needs Ionic.Zip.DLL in the same folder to run unzip_pc3.exe

---------------------------------------------------------------
Original Version: Boxa Shu 2014-Okt-16

v1.01 Arie van Brakel 2020-Jan-16

-Fixed  Special chars in strings #1

	If special chars like é are replaced by ?
	
	-Added Example File: NLHAG1-PLO0103-Black_White_NON_FOLDING-for_AutoCAD.PC3
	
-Added error messages and userfeedback

-Added filter on files

-Added support for extracting .PMP and .CTB files

-Added Ionic.Zip.DLL (Needed for Excecution)

-Be aware string lines don't have closing " 

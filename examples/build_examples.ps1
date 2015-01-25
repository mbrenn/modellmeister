$dotNetPath = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319"

Write-Output "Creates the C# files"

Copy-Item ..\bin\ModellMeister.Library.dll ..\examples\mbgi\ModellMeister.Library.dll

..\bin\mbgi2cs.exe -i ..\examples\mbgi\onlytype.mbgi -o ..\examples\cs\onlytype.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\twotypes.mbgi -o ..\examples\cs\twotypes.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\twoblocks.mbgi -o ..\examples\cs\twoblocks.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\twoblockswithnamespace.mbgi -o ..\examples\cs\twoblockswithnamespace.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\fourblocks.mbgi -o ..\examples\cs\fourblocks.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\compositeblock.mbgi -o ..\examples\cs\compositeblock.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\autogenerationblock.mbgi -o ..\examples\cs\autogenerationblock.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\defaultvalue.mbgi -o ..\examples\cs\defaultvalue.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\import.mbgi -o ..\examples\cs\import.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\twonamespaces.mbgi -o ..\examples\cs\twonamespaces.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\importlibrary.mbgi -o ..\examples\cs\importlibrary.cs
..\bin\mbgi2cs.exe -i ..\examples\mbgi\feedback.mbgi -o ..\examples\cs\feedback.cs

Push-Location ..\examples\cs\

Write-Output "Compiles the C# files"
Write-Output "- onlytype"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\onlytype.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll onlytype.cs
Write-Output "- twotypes"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\twotypes.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll twotypes.cs
Write-Output "- import"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\import.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll import.cs
Write-Output "- twoblocks"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\twoblocks.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll twoblocks.cs
Write-Output "- twoblocks with namespace"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\twoblocks.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll twoblockswithnamespace.cs
Write-Output "- fourblocks"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\fourblocks.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll fourblocks.cs Implementation.cs
Write-Output "- twonamespaces"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\twonamespaces.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll twonamespaces.cs
Write-Output "- compositeblock"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\compositeblock.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll compositeblock.cs Implementation.cs
Write-Output "- autogenerationblock"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\autogenerationblock.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll autogenerationblock.cs autogenerationblock_code.cs
Write-Output "- defaultvalue"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\defaultvalue.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll defaultvalue.cs defaultvalue_Code.cs
 Write-Output "- feedback"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\feedback.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:../../bin/ModellMeister.Library.dll /r:System.Runtime.dll feedback.cs feedback_code.cs

Pop-Location

Copy-Item ..\bin\ModellMeister.Library.dll ..\examples\bin\ModellMeister.Library.dll

..\bin\mbsim.exe ..\examples\bin\feedback.dll

# [System.Console]::ReadKey()
 
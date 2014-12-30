$dotNetPath = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319"

Write-Output "Creates the C# files"

..\bin\mbgi2cs.exe ..\examples\mbgi\onlytype.mbgi ..\examples\cs\onlytype.cs
..\bin\mbgi2cs.exe ..\examples\mbgi\twotypes.mbgi ..\examples\cs\twotypes.cs
..\bin\mbgi2cs.exe ..\examples\mbgi\twoblocks.mbgi ..\examples\cs\twoblocks.cs
..\bin\mbgi2cs.exe ..\examples\mbgi\fourblocks.mbgi ..\examples\cs\fourblocks.cs
..\bin\mbgi2cs.exe ..\examples\mbgi\compositeblock.mbgi ..\examples\cs\compositeblock.cs

Push-Location ..\examples\cs\

Write-Output "Compiles the C# files"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\onlytype.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll onlytype.cs
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\twotypes.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll twotypes.cs
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\twoblocks.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll twoblocks.cs
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\fourblocks.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll fourblocks.cs Implementation.cs
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\compositeblock.dll /debug+ /r:../../bin/ModellMeister.Runtime.dll /r:System.Runtime.dll compositeblock.cs Implementation.cs

Pop-Location

..\bin\mbsim.exe ..\examples\bin\compositeblock.dll

# [System.Console]::ReadKey()
 
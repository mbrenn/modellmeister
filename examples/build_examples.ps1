$dotNetPath = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319"

Write-Output "Creates the C# files"

..\bin\mbgi2cs.exe ..\examples\mbgi\onlytype.mbgi ..\examples\cs\onlytype.cs
..\bin\mbgi2cs.exe ..\examples\mbgi\twotypes.mbgi ..\examples\cs\twotypes.cs
..\bin\mbgi2cs.exe ..\examples\mbgi\twoblocks.mbgi ..\examples\cs\twoblocks.cs
..\bin\mbgi2cs.exe ..\examples\mbgi\fourblocks.mbgi ..\examples\cs\fourblocks.cs

Push-Location ..\examples\cs\

Write-Output "Compiles the C# files"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\onlytype.dll /debug+ onlytype.cs
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\twotypes.dll /debug+ twotypes.cs
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\twoblocks.dll /debug+ twoblocks.cs
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\fourblocks.dll /debug+ fourblocks.cs Implementation.cs

Pop-Location

# [System.Console]::ReadKey()
 
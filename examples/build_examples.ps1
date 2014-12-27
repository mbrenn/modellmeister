$dotNetPath = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319"

Write-Output "Creates the C# files"

..\bin\mbgi2cs.exe ..\examples\mbgi\onlytype.mbgi ..\examples\cs\onlytype.cs
..\bin\mbgi2cs.exe ..\examples\mbgi\twotypes.mbgi ..\examples\cs\twotypes.cs

Push-Location ..\examples\cs\

Write-Output "Compiles the C# files"
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\onlytype.dll /debug+ onlytype.cs
."$dotNetPath\csc.exe" /nologo /target:library /out:..\bin\twotypes.dll /debug+ twotypes.cs

Pop-Location

# [System.Console]::ReadKey()

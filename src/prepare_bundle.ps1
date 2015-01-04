$7zipExe = "c:\Program Files\7-Zip\7z.exe"
$dotNetPath = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319"

if (Test-Path ../bin){
  Remove-Item -Recurse ../bin
}

New-Item -ItemType directory -Path ../bin

if (Test-Path ../dist){
  Remove-Item -Recurse ../dist
}

New-Item -ItemType directory -Path ../dist

."$dotNetPath\msbuild" /m /p:Configuration=Release ModellMeister.sln

Push-Location ../bin/

."$7zipExe" a -r ../dist/modellmeister.zip *

Pop-Location

Write-Host "Creation of Zip-File is done in /dist-Directory. Do not forget to create a tag in git"
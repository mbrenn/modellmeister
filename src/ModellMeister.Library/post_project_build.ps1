Write-Host "Copy files to the bin-directory of mbgi2cs"



# Copies the file to the bin/Debug of mbgi2cs
if (-Not (Test-Path "$($args[2])mbgi2cs\bin\Debug")) {
	New-Item -ItemType directory -Path "$($args[2])mbgi2cs\bin\Debug"
}

Write-Host "$($args[1])*.*" "$($args[2])mbgi2cs\bin\Debug"
Copy-Item "$($args[1])*.*" "$($args[2])mbgi2cs\bin\Debug"
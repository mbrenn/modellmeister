Write-Host "Copy files to the bin-directory"

if (-Not (Test-Path "$($args[2])../bin")) {
	New-Item -ItemType directory -Path "$($args[2])../bin"
}


Write-Host "$($args[1])*.*" "$($args[2])..\bin\"
Copy-Item "$($args[1])*.*" "$($args[2])..\bin\"
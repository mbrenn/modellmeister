Write-Host "Copy files to the bin-directory"

if (-Not (Test-Path "$($args[2])../bin")) {
	New-Item -ItemType directory -Path "$($args[2])../bin"
}

if (-Not (Test-Path "$($args[2])../bin/examples")) {
	New-Item -ItemType directory -Path "$($args[2])../bin/examples"
}

Write-Host "$($args[1])*.*" "$($args[2])..\bin\"
Copy-Item "$($args[1])*.*" "$($args[2])..\bin\"

if ((Test-Path "$($args[1])examples\*.*"))
{
	Copy-Item "$($args[1])examples\*.*" "$($args[2])..\bin\examples"
}
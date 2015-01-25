Write-Host "Create the C#-Files for the entityfiles"

."$($args[2])..\bin\mbgi2cs.exe" -i "$($args[0])all.mbgi" -o "$($args[0])all.cs"
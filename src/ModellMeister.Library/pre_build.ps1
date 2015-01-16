Write-Host "Create the C#-Files for the entityfiles"

."$($args[2])..\bin\mbgi2cs.exe" "$($args[0])all.mbgi" "$($args[0])all.cs"
Write-Host "Create the C#-Files for the entityfiles"

."$($args[2])..\bin\mbgi2cs.exe" "$($args[0])Algebra.mbgi" "$($args[0])Algebra.cs"
."$($args[2])..\bin\mbgi2cs.exe" "$($args[0])Analysis.mbgi" "$($args[0])Analysis.cs"
."$($args[2])..\bin\mbgi2cs.exe" "$($args[0])Comparison.mbgi" "$($args[0])Comparison.cs"
."$($args[2])..\bin\mbgi2cs.exe" "$($args[0])ControlFlow.mbgi" "$($args[0])ControlFlow.cs"
."$($args[2])..\bin\mbgi2cs.exe" "$($args[0])Helper.mbgi" "$($args[0])Helper.cs"
."$($args[2])..\bin\mbgi2cs.exe" "$($args[0])Logic.mbgi" "$($args[0])Logic.cs"
."$($args[2])..\bin\mbgi2cs.exe" "$($args[0])Statistics.mbgi" "$($args[0])Statistics.cs"


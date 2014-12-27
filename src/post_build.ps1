Write-Host "$($args[1])*.* $($args[2])..\bin\"
Copy-Item "$($args[1])*.*" "$($args[2])..\bin\"
Write-Host "Copy files to the bin-directory"
Copy-Item "$($args[1])*.*" "$($args[2])..\bin\"
param(
    [string]$server,
    [string]$version
)

Write-Host "Provided server url: $server"
Write-Host "Provided version: $version"

$containerUrl = $server.Trim('"')
$uri = "$containerUrl/version"

Write-Host "Checking version at $uri"

# Delay to ensure the new container app has been deployed
Start-Sleep -Seconds 60
Invoke-WebRequest $uri -UseBasicParsing | Foreach {
    $_.Content.Contains($version) | Foreach {
        if(-Not($_)) {
            Throw "Incorrect version."
        }
        else {
            Write-Host "Correct version: $version"
        }
    }
}
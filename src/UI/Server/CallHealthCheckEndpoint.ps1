param(
    [string]$server
)

Write-Host "Provided server url: $server"
$containerUrl = $server.Trim('"')

Start-Sleep -Seconds 90
$uri = "$containerUrl/_healthcheck"
Write-Host "Smoke testing $uri"

Invoke-WebRequest $uri -UseBasicParsing | Foreach {
    $_.Content.Contains("Healthy") | Foreach {
        if(-Not($_)) {
            Throw "Web smoke test failed"
        }
        else {
            Write-Host "Web smoke test passed."
        }
    }
}
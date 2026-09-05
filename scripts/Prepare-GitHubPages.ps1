param(
	[Parameter(Mandatory = $true)]
	[string]$PublishPath,

	[Parameter(Mandatory = $true)]
	[string]$RepositoryName
)

$ErrorActionPreference = 'Stop'

$wwwrootPath = Join-Path $PublishPath 'wwwroot'
if (-not (Test-Path $wwwrootPath)) {
	throw "Published wwwroot folder was not found at '$wwwrootPath'."
}

$sitePath = "/$RepositoryName/"
$indexPath = Join-Path $wwwrootPath 'index.html'
$manifestPath = Join-Path $wwwrootPath 'manifest.webmanifest'
$serviceWorkerPath = Join-Path $wwwrootPath 'service-worker.js'
$notFoundPath = Join-Path $wwwrootPath '404.html'
$noJekyllPath = Join-Path $wwwrootPath '.nojekyll'

$indexHtml = Get-Content -Path $indexPath -Raw
$indexHtml = $indexHtml.Replace('<base href="/" />', "<base href=`"$sitePath`" />")
Set-Content -Path $indexPath -Value $indexHtml -NoNewline

$manifest = Get-Content -Path $manifestPath -Raw | ConvertFrom-Json
$manifest.id = $sitePath
$manifest.start_url = $sitePath
$manifest.scope = $sitePath
$manifestJson = $manifest | ConvertTo-Json -Depth 10
Set-Content -Path $manifestPath -Value $manifestJson -NoNewline

if (Test-Path $serviceWorkerPath) {
	$serviceWorker = Get-Content -Path $serviceWorkerPath -Raw
	$serviceWorker = $serviceWorker.Replace('const base = "/";', "const base = `"$sitePath`";")
	Set-Content -Path $serviceWorkerPath -Value $serviceWorker -NoNewline
}

Copy-Item -Path $indexPath -Destination $notFoundPath -Force
New-Item -Path $noJekyllPath -ItemType File -Force | Out-Null

Write-Host "Prepared GitHub Pages output for base path $sitePath"

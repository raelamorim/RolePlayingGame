Param(
    [string]$SolutionPath = ".",
    [string]$TestProjectPattern = "*UnitTest*.csproj",
    [string]$TargetProjectName = "RolePlayingGame.Domain.csproj"
)

Write-Host "Starting mutation tests..."

# Test project
$testProject = Get-ChildItem -Path $SolutionPath -Recurse -Filter $TestProjectPattern | Select-Object -First 1
if (-not $testProject) {
    Write-Host "No test project found."
    exit 1
}

Write-Host "Using test project: $($testProject.Name)"

# Domain project
$targetProject = Get-ChildItem "$SolutionPath\src" -Recurse -Filter $TargetProjectName
if (-not $targetProject) {
    Write-Host "Target project not found: $TargetProjectName"
    exit 1
}

Write-Host "Mutating: $($targetProject.Name)"

$projectDir = Split-Path $testProject.FullName -Parent
Push-Location $projectDir

dotnet stryker --project $targetProject.Name --reporter html
$exitCode = $LASTEXITCODE

Pop-Location

Write-Host ""
Write-Host "RESULT"
Write-Host "---------------------"

if ($exitCode -eq 0) {
    Write-Host "$($targetProject.Name) - OK"
}
else {
    Write-Host "$($targetProject.Name) - FAILED ($exitCode)"
}

Write-Host ""
Write-Host "Done."

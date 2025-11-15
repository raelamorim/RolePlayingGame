# 1. Executa testes usando o arquivo .runsettings
dotnet test `
  --settings .\coverlet.runsettings

# 2. Procura TODOS os arquivos de cobertura
$coverageFiles = Get-ChildItem -Recurse -Filter coverage.cobertura.xml

if ($coverageFiles.Count -eq 0) {
    Write-Host "❌ Nenhum arquivo coverage.cobertura.xml encontrado!"
    exit 1
}

Write-Host "📄 Arquivos encontrados:"
$coverageFiles | ForEach-Object { Write-Host " - $($_.FullName)" }

# Junta todos os caminhos separados por ponto e vírgula (formato aceito pelo ReportGenerator)
$reportsArgument = ($coverageFiles.FullName -join ";")

# 3. Gera relatório HTML combinando tudo
reportgenerator `
  -reports:$reportsArgument `
  -targetdir:"coverage-report" `
  -reporttypes:Html

Write-Host "✅ Relatório combinado gerado em ./coverage-report/index.html"

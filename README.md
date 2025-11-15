# RolePlayingGame 🎲⚔️

Sample .NET 8 project to manage characters and simulate battles (API + Application/Domain/Infrastructure layers + unit & integration tests). 🚀

## Checkout the repository 👇
Clone and open the repo locally:
```bash
git clone https://github.com/raelamorim/RolePlayingGame.git
cd RolePlayingGame
dotnet restore
```

## Status ✅
- API: src/RolePlayingGame.Api/RolePlayingGame.Api.csproj  
- Unit tests: test/RolePlayingGame.UnitTest/RolePlayingGame.UnitTest.csproj 🧪  
- Integration tests: test/RolePlayingGame.IntegrationTest/RolePlayingGame.IntegrationTest.csproj 🔁

## Quick design overview 🧭
- Controllers: POST/GET — CharacterController, BattleController ⚔️  
- Application use cases: PostCharacterUseCase, PostBattleUseCase, GetCharacterListUseCase, GetCharacterDetailUseCase 🛠️  
- Main entity: Character (with Job enum) 👤  
- Battle logic: ValueObject Battle ⚔️  
- Infrastructure (EF InMemory): CharacterRepository, RolePlayingGameDbContext 🗄️  
- Mappers for DTOs: PostCharacterMapper, GetCharacterListMapper, GetCharacterDetailMapper, PostBattleMapper 🔄

## How to run locally 🏃‍♂️
Prerequisites:
- .NET 8 SDK

Run the API:
- From the solution root:
  - dotnet build
  - dotnet run --project src/RolePlayingGame.Api

Main host entry: src/RolePlayingGame.Api/Program.cs. Startup is in src/RolePlayingGame.Api/Startup.cs. 🔧

## Developer tools — Stryker (mutation) & Coverlet (coverage) 🧰
Install as global tools (quick, system-wide):
```powershell
# Stryker.NET (mutation testing)
dotnet tool install --global dotnet-stryker

# Coverlet console (coverage tooling)
dotnet tool install --global coverlet.console
```

Or install locally in the repository (recommended for CI consistency):
```powershell
# create a local tool manifest (if not present)
dotnet new tool-manifest

# install tools to the manifest (local tools)
dotnet tool install dotnet-stryker
dotnet tool install coverlet.console
```

How to run:
- Stryker (mutation testing):
  - If installed globally: dotnet stryker
  - If installed as local tool: dotnet tool run dotnet-stryker
- Coverlet (console) — example of running coverage for a test DLL:
  - coverlet <path-to-test-assembly> --target "dotnet" --targetargs "test --no-build" --output "coverage.json" --format "lcov"
  - If using local tool: dotnet tool run coverlet ...

Notes:
- The repository includes a PowerShell coverage script (.\run-tests-with-coverage.ps1). Install the tools (global or local) above so the script can call the required tools. 🛠️
- For CI, prefer local tool manifest to lock tool versions.

## Tests 🧪
Run all tests:
- dotnet test

Coverage script (Windows PowerShell):
- .\run-tests-with-coverage.ps1 — uses coverlet and ReportGenerator to emit coverage-report/index.html 📊

Key test files:
- Unit: test/RolePlayingGame.UnitTest/Domain/Entities/CharacterTests.cs  
- Integration (BDD): test/RolePlayingGame.IntegrationTest/Features/RolePlayingGame.feature and step definitions in test/RolePlayingGame.IntegrationTest/Steps/

## Main endpoints 📎
- POST /characters — create character (validations in PostCharacterRequest) ➕  
- GET /characters — list characters 📋  
- GET /characters/{id} — character detail 🔍  
- POST /battles — run battle between two character ids ⚔️

Controllers:
- src/RolePlayingGame.Api/Controllers/CharacterController.cs  
- src/RolePlayingGame.Api/Controllers/BattleController.cs

## Repository layout 📁
- src/RolePlayingGame.Api/ — Web API (Program.cs, Startup.cs)  
- src/RolePlayingGame.Application/ — Use cases, DTOs, mappers  
- src/RolePlayingGame.Domain/ — Entities, value objects, enums  
- src/RolePlayingGame.Infrastructure/ — Configuration and EF repositories  
- test/ — Unit and integration tests (BDD)

## Notes & best practices 💡
- Infrastructure uses InMemory DB for development by default (see InfrastructureConfiguration). 🧰  
- Domain errors (e.g., player not found) throw application exceptions and are mapped by GlobalExceptionMiddleware. ⚠️  
- Keep mappers thin and cover mapping/domain rules with tests. ✅

## Docker 🐳
Build and run:
- cd src/RolePlayingGame.Api
- docker build -t roleplayinggame-api .
- docker run --rm roleplayinggame-api

Use volumes or environment variables to map local input/output if needed.

## Contributing 🤝
- Fork → branch → PR  
- Run tests locally before submitting  
- Keep PRs small and focused; include tests for behavior changes

## Links (quick) 🔗
- src/RolePlayingGame.Api/Controllers/CharacterController.cs  
- src/RolePlayingGame.Api/Controllers/BattleController.cs  
- src/RolePlayingGame.Api/Startup.cs  
- src/RolePlayingGame.Api/Program.cs  
- src/RolePlayingGame.Application/UseCase/PostCharacterUseCase.cs  
- src/RolePlayingGame.Application/UseCase/PostBattleUseCase.cs  
- src/RolePlayingGame.Domain/Entities/Character.cs  
- src/RolePlayingGame.Domain/ValueObjects/Battle.cs  
- src/RolePlayingGame.Infrastructure/Databases/Repositories/CharacterRepository.cs  
- test/RolePlayingGame.UnitTest/Domain/Entities/CharacterTests.cs  
- test/RolePlayingGame.IntegrationTest/Features/RolePlayingGame.feature

Enjoy coding! 🎉
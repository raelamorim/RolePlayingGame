FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy solution and project files to leverage layer caching
COPY *.sln ./
COPY src/RolePlayingGame.Api/*.csproj src/RolePlayingGame.Api/
COPY src/RolePlayingGame.Application/*.csproj src/RolePlayingGame.Application/
COPY src/RolePlayingGame.Domain/*.csproj src/RolePlayingGame.Domain/
COPY src/RolePlayingGame.Infrastructure/*.csproj src/RolePlayingGame.Infrastructure/

# include test project csproj files so 'dotnet restore' finds them referenced by the solution
COPY test/RolePlayingGame.UnitTest/*.csproj test/RolePlayingGame.UnitTest/
COPY test/RolePlayingGame.IntegrationTest/*.csproj test/RolePlayingGame.IntegrationTest/

RUN dotnet restore

# copy remaining sources and publish
COPY . .
RUN dotnet publish src/RolePlayingGame.Api/RolePlayingGame.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "RolePlayingGame.Api.dll"]

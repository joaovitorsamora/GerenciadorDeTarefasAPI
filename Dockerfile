FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["GerenciadorDeTarefas.sln", "."]
COPY ["GerenciadorDeTarefas/", "GerenciadorDeTarefas/"]
RUN dotnet restore "GerenciadorDeTarefas.sln"
RUN dotnet publish "GerenciadorDeTarefas.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GerenciadorDeTarefas.dll"]

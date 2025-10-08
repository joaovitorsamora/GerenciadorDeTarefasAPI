# Etapa base - runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GerenciadorDeTarefas/GerenciadorDeTarefas.csproj", "GerenciadorDeTarefas/"]
RUN dotnet restore "GerenciadorDeTarefas/GerenciadorDeTarefas.csproj"
COPY . .
WORKDIR "/src/GerenciadorDeTarefas"
RUN dotnet build "GerenciadorDeTarefas.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GerenciadorDeTarefas.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Render usa sempre a variável PORT para o binding
ENV ASPNETCORE_URLS=http://+:$PORT
ENTRYPOINT ["dotnet", "GerenciadorDeTarefas.dll"]

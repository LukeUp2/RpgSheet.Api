# Estágio 1: Build
# Usando o SDK 8.0 para garantir compatibilidade com o runtime 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# O segredo aqui é usar caminhos relativos ao contexto de build.
# Se o arquivo está em backend/RpgSheet.Api/RpgSheet.Api.csproj, 
# vamos copiar mantendo a estrutura para o restore funcionar.
COPY ["backend/RpgSheet.Api/RpgSheet.Api.csproj", "backend/RpgSheet.Api/"]
RUN dotnet restore "backend/RpgSheet.Api/RpgSheet.Api.csproj"

# Copia todo o conteúdo do repositório
COPY . .

# Muda para a pasta do projeto para o build e publish
WORKDIR "/src/backend/RpgSheet.Api"
RUN dotnet publish "RpgSheet.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 2: Runtime Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Fly.io usa a porta 8080 por padrão para .NET 8+
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RpgSheet.Api.dll"]
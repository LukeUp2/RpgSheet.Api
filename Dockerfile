# Estágio 1: Build e Publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todos os arquivos para garantir que referências a outros projetos 
# (se houver) funcionem, mas foca no projeto principal.
COPY . .

# O segredo: Procurar o .csproj de forma recursiva e restaurar
# Isso evita erros se você estiver na pasta raiz ou na pasta backend/
RUN dotnet restore **/*.csproj

# Compila e publica o projeto apontando para o arquivo específico
# O publish já faz o build internamente
RUN dotnet publish **/RpgSheet.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Estágio 2: Runtime Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Fly.io usa 8080 para .NET 8
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RpgSheet.Api.dll"]
# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia os arquivos de projeto primeiro para aproveitar o cache das camadas (Restore)
COPY ["backend/RpgSheet.Api/RpgSheet.Api.csproj", "backend/RpgSheet.Api/"]
RUN dotnet restore "backend/RpgSheet.Api/RpgSheet.Api.csproj"

# Copia o restante dos arquivos e compila
COPY . .
WORKDIR "/src/backend/RpgSheet.Api"
RUN dotnet build "RpgSheet.Api.csproj" -c Release -o /app/build

# Estágio 2: Publish
FROM build AS publish
RUN dotnet publish "RpgSheet.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 3: Runtime Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expor a porta que o Kestrel usa (geralmente 80 ou 8080 no .NET 8)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RpgSheet.Api.dll"]
## Use a imagem base do .NET SDK para o runtime
#FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
## Use a imagem base do .NET SDK para compilar o projeto
#FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["TaskManagerAPI/TaskManagerAPI.csproj", "TaskManagerAPI/"]
#RUN dotnet restore "TaskManagerAPI/TaskManagerAPI.csproj"
#COPY . .
#WORKDIR "/src/TaskManagerAPI"
#RUN dotnet build "TaskManagerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## Publicar o projeto
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "TaskManagerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## Configurar a imagem final
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "TaskManagerAPI.dll"]
#


# Use a imagem base do .NET SDK para o runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use a imagem base do .NET SDK para compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Linha corrigida: Copia o arquivo TaskManagerAPI.csproj para a pasta correta dentro do cont�iner
COPY ["TaskManagerAPI/TaskManagerAPI.csproj", "TaskManagerAPI/"]
RUN dotnet restore "TaskManagerAPI/TaskManagerAPI.csproj"

COPY . . 
WORKDIR "/src/TaskManagerAPI"
RUN dotnet build "TaskManagerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar o projeto
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TaskManagerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Configurar a imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "TaskManagerAPI.dll"]

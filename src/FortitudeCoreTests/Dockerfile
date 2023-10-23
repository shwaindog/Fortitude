#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/runtime:7.0.13-jammy-amd64 AS base
FROM mcr.microsoft.com/dotnet/sdk:7.0.403-jammy-amd64 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0.403-jammy-amd64 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["FortitudeCoreTests/FortitudeCoreTests.csproj", "FortitudeCoreTests/"]
COPY ["FortitudeCoreCommon/FortitudeCoreCommon.csproj", "FortitudeCoreCommon/"]
COPY ["FortitudeCoreIO/FortitudeCoreIO.csproj", "FortitudeCoreIO/"]
COPY ["FortitudeCoreMarketsApi/FortitudeCoreMarketsApi.csproj", "FortitudeCoreMarketsApi/"]
COPY ["FortitudeCoreMarketsCore/FortitudeCoreMarketsCore.csproj", "FortitudeCoreMarketsCore/"]
RUN dotnet restore "FortitudeCoreTests/FortitudeCoreTests.csproj"
COPY . .
WORKDIR "/src/FortitudeCoreTests"
RUN dotnet build "FortitudeCoreTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FortitudeCoreTests.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "FortitudeCoreTests.dll"]
 ENTRYPOINT ["/bin/bash"]
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

RUN dotnet tool install dotnet-ef --tool-path /home/app/.dotnet/tools
RUN chown -R app /home/app/.dotnet
ENV PATH="$PATH:/home/app/.dotnet/tools"

WORKDIR /src
COPY ["src/SitesAdmin/SitesAdmin.csproj", "SitesAdmin/"]
RUN dotnet restore "./SitesAdmin/SitesAdmin.csproj"
COPY . .

RUN dotnet build "./src/SitesAdmin/SitesAdmin.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./src/SitesAdmin/SitesAdmin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
RUN dotnet ef migrations bundle --project "./src/SitesAdmin/SitesAdmin.csproj" -o /app/publish/efbundle.exe

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /home/app/.aspnet/DataProtection-Keys

ENTRYPOINT ["dotnet", "SitesAdmin.dll"]
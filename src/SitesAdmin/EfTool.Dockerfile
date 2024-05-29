#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

#COPY ["./ef-commands.sh", "/app"]

#RUN chmod +x /app/ef-commands.sh

##RUN dotnet restore "./SitesAdmin.csproj"
##RUN dotnet build "./SitesAdmin.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR /src/SitesAdmin

ENTRYPOINT ["dotnet", "ef"]
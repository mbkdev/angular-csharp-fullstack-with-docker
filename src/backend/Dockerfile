# Build
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution
COPY Backend.sln ./ 

# Copy .csproj-Files
COPY ./data/data.csproj data/
COPY ./core/core.csproj core/ 
COPY ./backend/backend.csproj backend/ 

# Restore complete project
RUN dotnet restore Backend.sln

# Copy all files
COPY . .

WORKDIR /src/backend

# Publish as release
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "backend.dll"]
# 1. Base Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. SDK Build Image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["TechMoveGLMS.API.csproj", "./"]
RUN dotnet restore "TechMoveGLMS.API.csproj"

# Copy remaining files and build
COPY . .
RUN dotnet build "TechMoveGLMS.API.csproj" -c Release -o /app/build

# 3. Publish application
FROM build AS publish
RUN dotnet publish "TechMoveGLMS.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. Final Execution
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechMoveGLMS.API.dll"]
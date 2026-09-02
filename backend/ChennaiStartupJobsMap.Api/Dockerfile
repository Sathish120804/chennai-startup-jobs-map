# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ChennaiStartupJobsMap.Api.csproj", "."]
RUN dotnet restore "ChennaiStartupJobsMap.Api.csproj"

COPY . .
RUN dotnet build "ChennaiStartupJobsMap.Api.csproj" -c Release -o /app/build
RUN dotnet publish "ChennaiStartupJobsMap.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ChennaiStartupJobsMap.Api.dll"]

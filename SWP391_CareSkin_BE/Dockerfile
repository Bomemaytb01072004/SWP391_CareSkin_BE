# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .  
WORKDIR /app/SWP391_CareSkin_BE

# Restore dependencies
RUN dotnet restore

# Build & Publish project
RUN dotnet publish -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy output từ build stage
COPY --from=build /publish .

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "SWP391_CareSkin_BE.dll"]

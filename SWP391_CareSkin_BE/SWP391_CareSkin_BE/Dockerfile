# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["SWP391_CareSkin_BE.csproj", "./"]
RUN dotnet restore "./SWP391_CareSkin_BE.csproj"

# Copy the remaining files and build the project
COPY . .
RUN dotnet publish "SWP391_CareSkin_BE.csproj" -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "SWP391_CareSkin_BE.dll"]

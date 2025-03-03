FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SWP391_CareSkin_BE.csproj", "./"]
RUN dotnet restore "./SWP391_CareSkin_BE.csproj"

COPY . .
RUN dotnet publish "SWP391_CareSkin_BE.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Copy firebasekey.json into the container
COPY firebasekey.json /app/firebasekey.json

EXPOSE 80

ENTRYPOINT ["dotnet", "SWP391_CareSkin_BE.dll"]

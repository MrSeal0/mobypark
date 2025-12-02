FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["API-C#/API-C#.csproj", "API-C#/"]
RUN dotnet restore "API-C#/API-C#.csproj"

COPY . .
WORKDIR "/src/API-C#"
RUN dotnet publish "API-C#.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "API-C#.dll"]
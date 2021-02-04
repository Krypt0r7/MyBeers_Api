FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["MyBeers.Api/MyBeers.Api.csproj", "src/MyBeers.Api/"]
RUN dotnet restore "src/MyBeers.Api/MyBeers.Api.csproj"
COPY . .
WORKDIR "/src/src/MyBeers.Api"
RUN dotnet build "MyBeers.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyBeers.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyBeers.Api.dll"]

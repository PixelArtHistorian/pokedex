FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-environment
WORKDIR /app

COPY src ./
RUN dotnet restore PokedexApi.sln
RUN dotnet publish PokedexApi.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-environment /app/out .
EXPOSE 1025
ENV ASPNETCORE_HTTP_PORTS=1025

ENTRYPOINT ["dotnet", "PokedexApi.dll"]
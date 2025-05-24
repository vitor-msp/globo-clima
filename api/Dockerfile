FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /api
COPY *.sln .
COPY GloboClima.Api ./GloboClima.Api
RUN dotnet sln remove "GloboClima.Tests\GloboClima.Tests.csproj"
RUN dotnet restore
RUN dotnet publish -c Release -o build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /api
COPY --from=build /api/build .
EXPOSE 5000
ENTRYPOINT ["dotnet", "GloboClima.Api.dll"]
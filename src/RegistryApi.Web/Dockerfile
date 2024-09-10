FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY / .
RUN dotnet build -c Release -o /app/build

FROM build AS test
RUN dotnet test -c Release --logger "trx;LogFileName=testresults.trx"

FROM build AS publish
RUN dotnet publish "Web/Web.Api/Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RegistryApi.Web.Api.dll"]
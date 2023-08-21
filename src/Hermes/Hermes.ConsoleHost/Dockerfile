FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Hermes.ConsoleHost/Hermes.ConsoleHost.csproj Hermes.ConsoleHost/
RUN dotnet restore Hermes.ConsoleHost/Hermes.ConsoleHost.csproj
COPY . .
WORKDIR /src/Hermes.ConsoleHost
RUN dotnet build Hermes.ConsoleHost.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Hermes.ConsoleHost.csproj -c Release -o /app

FROM base AS final
EXPOSE 1883
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Hermes.ConsoleHost.dll"]

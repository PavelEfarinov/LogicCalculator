FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LogicServer/", "LogicServer/"]
COPY ["DbConnector/", "DbConnector/"]
COPY ["DbMigrator/", "DbMigrator/"]
COPY ["Logic.Dto/", "Logic.Dto/"]
WORKDIR "/src/LogicServer"
RUN dotnet restore "LogicServer.csproj"
RUN dotnet build "LogicServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogicServer.csproj" -c Release -o /app/publish

FROM base AS final-app-server
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogicServer.dll"]

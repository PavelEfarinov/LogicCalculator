FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Logic.Server/", "Logic.Server/"]
COPY ["Db.Connector/", "Db.Connector/"]
COPY ["Db.Migrator/", "Db.Migrator/"]
COPY ["Logic.Dto/", "Logic.Dto/"]
COPY ["Logic.Utils/", "Logic.Utils/"]
COPY ["Logic.Proto/", "Logic.Proto/"]
WORKDIR "/src/LogicServer"
RUN dotnet restore "LogicServer.csproj"
RUN dotnet build "LogicServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogicServer.csproj" -c Release -o /app/publish

FROM base AS final-app-server
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogicServer.dll"]

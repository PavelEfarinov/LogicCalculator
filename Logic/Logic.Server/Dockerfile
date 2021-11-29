FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Logic/Logic.Server/", "Logic/Logic.Server/"]
COPY ["Db/Db.Connector/", "Db/Db.Connector/"]
COPY ["Db/Db.Migrator/", "Db/Db.Migrator/"]
COPY ["Logic/Logic.Dto/", "Logic/Logic.Dto/"]
COPY ["Logic/Logic.Utils/", "Logic/Logic.Utils/"]
COPY ["Logic/Logic.Proto/", "Logic/Logic.Proto/"]
WORKDIR "/src/Logic/Logic.Server"
RUN dotnet restore "Logic.Server.csproj"
RUN dotnet build "Logic.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Logic.Server.csproj" -c Release -o /app/publish

FROM base AS final-app-server
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Logic.Server.dll"]

# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Render define a porta via variável de ambiente PORT
ENV ASPNETCORE_URLS=http://*:$PORT
EXPOSE $PORT

ENTRYPOINT ["dotnet", "ControleFuncionarios.dll"]


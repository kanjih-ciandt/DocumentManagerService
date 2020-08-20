FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY documentManagerService.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app

RUN mkdir -p /files/UploadedFiles

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "documentManagerService.dll"]


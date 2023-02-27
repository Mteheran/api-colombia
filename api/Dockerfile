FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine
ENV DOTNET_EnableDiagnostics=0
RUN apk update && apk add bash
WORKDIR /api

COPY *.csproj .
COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o /api/out/
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "/api/bin/Release/net6.0/api.dll"]
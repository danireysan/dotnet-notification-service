FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

RUN apt-get update && apt-get install -y libkrb5-3 && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:5143
EXPOSE 5143

ENTRYPOINT ["dotnet", "dotnet-notification-service.dll"]
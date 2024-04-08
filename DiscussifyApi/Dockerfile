FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS="http://*:80"
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DiscussifyApi.csproj", "./"]
RUN dotnet restore "./DiscussifyApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DiscussifyApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DiscussifyApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DiscussifyApi.dll"]

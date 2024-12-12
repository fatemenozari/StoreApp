FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OnlineStore.csproj", "./"]
RUN dotnet restore "OnlineStore.csproj"
COPY . . 
WORKDIR "/src"
RUN dotnet build "OnlineStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineStore.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_URLS=http://*:8080
ENV ASPNETCORE_HTTP_PORTS=8080
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "OnlineStore.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MiniErp.Api/MiniErp.Api.csproj", "MiniErp.Api/"]
RUN dotnet restore "MiniErp.Api/MiniErp.Api.csproj"
COPY . .
WORKDIR "/src/MiniErp.Api"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "MiniErp.Api.dll"]

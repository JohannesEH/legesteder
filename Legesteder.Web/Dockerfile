FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Legesteder.Web/Legesteder.Web.csproj", "Legesteder.Web/"]
RUN dotnet restore "Legesteder.Web/Legesteder.Web.csproj"
COPY . .
WORKDIR "/src/Legesteder.Web"
RUN dotnet build "Legesteder.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Legesteder.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Legesteder.Web.dll"]

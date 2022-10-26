FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MockWebApi/MockWebApi.csproj", "MockWebApi/"]
RUN dotnet restore "MockWebApi/MockWebApi.csproj"
COPY . .
WORKDIR "/src/MockWebApi"
RUN dotnet build "MockWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MockWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockWebApi.dll"]

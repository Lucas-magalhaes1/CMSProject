FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CMSProject/CMSProject.csproj", "CMSProject/"]
COPY ["CMS.Infrastructure/CMS.Infrastructure.csproj", "CMS.Infrastructure/"]
COPY ["CMS.Domain/CMS.Domain.csproj", "CMS.Domain/"]
COPY ["CMS.Application/CMS.Application.csproj", "CMS.Application/"]
RUN dotnet restore "CMSProject/CMSProject.csproj"
COPY . .
WORKDIR "/src/CMSProject"
RUN dotnet build "./CMSProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CMSProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMSProject.dll"]

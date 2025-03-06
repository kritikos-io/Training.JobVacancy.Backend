FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ENV ContinuousIntegrationBuild=true
WORKDIR /src
COPY "src/Training.JobVacancy.Web.Server/Training.JobVacancy.Web.Server.csproj" "src/Training.JobVacancy.Web.Server/"
COPY "src/Training.JobVacancy.Data/Training.JobVacancy.Data.csproj" "src/Training.JobVacancy.Data/"
COPY "src/Training.JobVacancy.Web.Models/Training.JobVacancy.Web.Models.csproj" "src/Training.JobVacancy.Web.Models/"
COPY "Directory.Packages.props" "."
COPY "Directory.Build.props" "."
RUN dotnet restore "src/Training.JobVacancy.Web.Server/Training.JobVacancy.Web.Server.csproj"

COPY .git/ .git/
COPY . .
WORKDIR "/src/src/Training.JobVacancy.Web.Server"
RUN dotnet build --no-restore "Training.JobVacancy.Web.Server.csproj" -c $BUILD_CONFIGURATION

FROM build AS test
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src"
RUN dotnet test --no-build -c $BUILD_CONFIGURATION

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/src/Training.JobVacancy.Web.Server"
RUN dotnet publish --no-build "Training.JobVacancy.Web.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
RUN apt update && apt install -y curl
USER $APP_UID
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT "dotnet" "Training.JobVacancy.Web.Server.dll"

HEALTHCHECK --interval=30s --timeout=5s --retries=3 CMD curl --silent --fail http://localhost:8080/health || exit 1

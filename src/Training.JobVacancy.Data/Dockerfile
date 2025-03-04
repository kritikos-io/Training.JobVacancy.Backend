FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY src/Training.JobVacancy.sln .
COPY src/Training.JobVacancy.Data/*.csproj src/Training.JobVacancy.Data/
COPY src/Training.JobVacancy.Web.Server/*.csproj src/Training.JobVacancy.Web.Server/

RUN dotnet restore
COPY . .

WORKDIR /app/src/Training.JobVacancy.Data

RUN dotnet tool install --global dotnet-ef && export PATH="$PATH:/root/.dotnet/tools"

RUN dotnet ef migrations bundle --startup-project ../Training.JobVacancy.Web.Server --project . --context TrainingJobVacancyDbContext --output /migrations-bundle

ENTRYPOINT ["/migrations-bundle", "--connection", "$DATABASE_CONNECTION_STRING"]

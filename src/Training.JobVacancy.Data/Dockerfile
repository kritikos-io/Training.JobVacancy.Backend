FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . .

RUN dotnet tool install --global dotnet-ef

ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet-ef migrations bundle --startup-project ../Training.JobVacancy.Web.Server --project . --context TrainingJobVacancyDbContext --output /migrations-bundle

ENTRYPOINT ["/migrations-bundle", "--connection", "$DATABASE_CONNECTION_STRING"]

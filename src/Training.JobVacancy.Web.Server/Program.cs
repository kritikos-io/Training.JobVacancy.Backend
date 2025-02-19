using Adaptit.Training.JobVacancy.Web.Server;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddJobVacancyServices();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();
app.MapEndpoints();

app.Run();

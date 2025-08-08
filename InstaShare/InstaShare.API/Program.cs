using Asp.Versioning;
using InstaShare.Infrastructure;
using InstaShare.Application.Entities;
using InstaShare.Application.Repositories.Interfaces;
using InstaShare.Application.Services;
using InstaShare.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using InstaShare.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy
                .WithOrigins(builder.Configuration.GetSection("AllowedHosts").Get<string>()!)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddScoped<IFilesRepository, FilesRepository>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddDbContext<FilesDataContext>(options =>
    options.UseInMemoryDatabase("InMemoryDatabase"));

builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "InstaShare Home Api"
    });

    options.TagActionsBy(api =>
    {
        if (api.GroupName != null) return new[] { api.GroupName };

        if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            return new[] { controllerActionDescriptor.ControllerName };

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });

    options.DocInclusionPredicate((_, _) => true);
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddHealthChecks();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<FilesDataContext>();

    // In real world do a proper migration, but here's the test data

    dbContext.Files.Add(new FileEntity
    {
        Id = new Guid("ff0c022e-1aff-4ad8-2231-08db0378ac98"),
        Name = "Default File",
        Status = "upload",
        Size = 1024
    });

    // Add more sample data
    dbContext.SeedInitialData();

    dbContext.SaveChanges();
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseResponseCompression();

app.MapControllers();

app.Run();

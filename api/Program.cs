using api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Colombia ",
        Description = "Un sitio para suministrar diferente informacion provista en API de Colombia.",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        }        
    });
});

builder.Services.AddNpgsql<DBContext>(builder.Configuration.GetConnectionString("csApiColombia"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbcreation", async ([FromServices] DBContext dbContext) => 
{
    dbContext.Database.EnsureCreated();
    return Results.Ok();
});

app.MapGet("/{id}", (int id) => $"This is the id:{id}!")
    .WithMetadata(new SwaggerOperationAttribute(summary: "This is the id endpoint", description: "This endpoint return the id information"));

app.UseSwagger();
app.UseSwaggerUI();


app.Run();

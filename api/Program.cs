using api;
using api.Routes;
using Microsoft.OpenApi.Models;

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

builder.Services.AddNpgsql<DBContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

InfoRoutes.RegisterInfoAPI(app);
CountryRoutes.RegisterCountryAPI(app);
DepartmentRoutes.RegisterDepartmentAPI(app);
CityRoutes.RegisterCityAPI(app);
PresidentRoutes.RegisterPresidentApi(app);
TuristicAttactionRoutes.RegisterTuristicAttactionAPI(app);

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

using System.Text.Json.Serialization;
using api;
using api.Routes;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http.Json;
using api.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0.1",
        Title = "API Ecuador ",
        Description = "Open and free API that contains general information about Ecuador",
        TermsOfService = new Uri("https://github.com/andresvillenas/api-ecuador"),
        Contact = new OpenApiContact
        {
            Name = "Andrés Villenas",
            Url = new Uri("http://avillenas.com")
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "corsApiEcuador",
                      policy  =>
                      {
                          policy.WithMethods("GET");
                          policy.AllowAnyOrigin();
                      });
});


builder.Configuration.AddEnvironmentVariables();

builder.Services.AddNpgsql<DBContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

var app = builder.Build();

InfoRoutes.RegisterInfoAPI(app);
CountryRoutes.RegisterCountryAPI(app);
ProvinceRoutes.RegisterProvinceAPI(app);
CityRoutes.RegisterCityAPI(app);
PresidentRoutes.RegisterPresidentApi(app);
TuristicAttactionRoutes.RegisterTuristicAttactionAPI(app);

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("corsApiEcuador");
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

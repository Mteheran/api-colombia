using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using api;
using AutoFixture;
using api.Models;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName;

    public CustomWebApplicationFactory()
    {
        // Generate a unique database name for each instance
        _databaseName = $"TestDatabase_{Guid.NewGuid()}";
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    { 
        builder.ConfigureServices(services =>
        {  
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DBContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
  
            services.AddDbContext<DBContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });
  
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();
            dbContext.Database.EnsureDeleted();   
            dbContext.Database.EnsureCreated();   

            var fixture = new Fixture();
 
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customize<DateOnly>(composer => composer.FromFactory(() => DateOnly.FromDateTime(DateTime.Now)));
            SeedDatabase(dbContext);
        });
    }

    private void SeedDatabase(DBContext dbContext)
    {
        
            var city1 = new City
            {
                Id = 1,
                Name = "Medellín",
                DepartmentId = 1,
            };

            var region = new Region
            {
                Id = 1,
                Name = "Andina",
                Description = "Región natural de Colombia",
                Departments = new List<Department>()
            };
            
            var deparment1 = new Department
            {
                Id = 1,
                Name = "Antioquia",
                NaturalAreas = new List<NaturalArea>(),
                CityCapitalId = 1,
                CityCapital = city1,
                Region = region,
                RegionId = region.Id,
                Cities = new List<City> {city1}
            };

            region.Departments = new List<Department> { deparment1 };
            city1.Department = deparment1;

            var categoryNaturalArea = new CategoryNaturalArea
            {
                Id = 1,
                Name = "Área Natural Única",
                Description = "Área geográfica que, por poseer condiciones especiales de flora o gea es un escenario natural raro."
            };

            var naturalArea = new NaturalArea
            {
                Id = 1,
                Name = "Parque Arví",
                DepartmentId = deparment1.Id,
                CategoryNaturalAreaId = categoryNaturalArea.Id,
                CategoryNaturalArea = categoryNaturalArea
            };

            categoryNaturalArea.NaturalAreas = new List<NaturalArea> { naturalArea };
            deparment1.NaturalAreas = new List<NaturalArea> { naturalArea };

            var touristAtraction = new TouristAttraction
            {
                Id = 1,
                Name = "Parque Explora",
                Description = "Parque temático de ciencia y tecnología",
                City = city1,
                CityId = city1.Id
            };

            city1.TouristAttractions = new List<TouristAttraction> { touristAtraction };
          
           var constitutionArticle = new ConstitutionArticle
            {
                Id = 1,
                TitleNumber = 1,
                Title = "DE LA RAMA EJECUTIVA",
                ChapterNumber = 1,
                Chapter = "DE LA FUNCION ADMINISTRATIVA",
                ArticleNumber = 1,
                Content = "La ley señalará las funciones que el Presidente de la República podrá elegir."
            }; 

            var airport = new Airport
            {
                Id = 1,
                Name = "Rionegro",
                CityId = 1,
                City = city1,
                DeparmentId = 1,
                Department = deparment1,
                IataCode = "MDE",
                OaciCode = "SKRG",
                Type = "Internacional",
            };

            var country = new Country
            {
                Id = 1,
                Name = "Colombia",
                InternetDomain = "CO",
                Departments = new List<Department> { deparment1 },
                Currency = "Peso",
                CurrencyCode = "COP",
                CurrencySymbol = "$",
                Flags = new string[] { "https://restcountries.com/data/col.svg" },
                Borders = new string[] { "BRA", "ECU", "PAN", "PER", "VEN" },
                Region = "Americas",
                Languages = new string[] { "Spanish" },
                Population = 50882891,
                Surface = 1141748,
            };

            if(!dbContext.Regions.Any())
            {
                dbContext.Add(region);
            }

            if(!dbContext.Departments.Any())
            {
                dbContext.Add(deparment1);
            }

            if(!dbContext.Cities.Any())
            {
                dbContext.Add(city1);
            }

            if (!dbContext.CategoryNaturalAreas.Any())
            {
                dbContext.Add(categoryNaturalArea);
                dbContext.Add(new CategoryNaturalArea
                {
                    Id = 2,
                    Name = "Área Natural Protegida",
                    Description = "Área geográfica que, por poseer condiciones especiales de flora o gea es un escenario natural raro.",
                    NaturalAreas = new List<NaturalArea>()
                });
                dbContext.Add(new CategoryNaturalArea
                {
                    Id = 3,
                    Name = "Área Natural de Interés Especial",
                    Description = "Área geográfica que, por poseer condiciones especiales de flora o gea es un escenario natural raro.",
                    NaturalAreas = new List<NaturalArea>()
                });
            }
            
            if (!dbContext.NaturalAreas.Any())
            {
                dbContext.Add(naturalArea);
            }

            if(!dbContext.TouristAttractions.Any())
            {
                dbContext.Add(touristAtraction);
            }
 
            if(!dbContext.ConstitutionArticles.Any())
            {
                dbContext.Add(constitutionArticle);
            } 

            if(!dbContext.Airports.Any())
            {
                dbContext.Add(airport);
            } 

            if(!dbContext.Countries.Any())
            {
                dbContext.Add(country);
            } 

            if(!dbContext.Maps.Any())
            {
                dbContext.Add(new Map
                {
                    Id = 1,
                    Name = "Mapa de Colombia",
                    Description = "Mapa de Colombia",
                    UrlSource = "https://www.google.com/maps/place/Colombia/@4.570868,-74.297333,5z/data=!3m1!4b1!4m6!3m5!1s0x8e3f99a2c9d7f2b7:0x4c8e2f8a2c9d7f2b7!8m2!3d4.570868!4d-74.297333!16zL20vMDJtZzQ?entry=ttu",
                    UrlImages = new List<string>
                    {
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Colombia_%28orthographic_projection%29.svg/1200px-Colombia_%28orthographic_projection%29.svg.png",
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4f/Colombia_location_map.svg/1200px-Colombia_location_map.svg.png"
                    }.ToArray()
                });
                dbContext.Add(new Map
                {
                    Id = 2,
                    Name = "Mapa de Medellín",
                    Description = "Mapa de Medellín",
                    UrlSource = "https://www.google.com/maps/place/Medell%C3%ADn,+Antioquia/@6.244203,-75.590654,12z/data=!3m1!4b1!4m6!3m5!1s0x8e442a2c9d7f2b7:0x4c8e2f8a2c9d7f2b7!8m2!3d6.244203!4d-75.590654!16zL20vMDJtZzQ?entry=ttu",
                    UrlImages = new List<string>
                    {
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Colombia_%28orthographic_projection%29.svg/1200px-Colombia_%28orthographic_projection%29.svg.png",
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4f/Colombia_location_map.svg/1200px-Colombia_location_map.svg.png"
                    }.ToArray()
                });
            }

            dbContext.SaveChanges();
    }

    public void ResetDatabase()
    {
        using (var scope = Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            SeedDatabase(dbContext);
        }
    }
}


using api.Models;
using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace api.Tests.ApiRoutesTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"TestDatabase_{Guid.NewGuid()}";

    // Generate a unique database name for each instance
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    { 
        builder.ConfigureServices(services =>
        {  
            // 1. Remove the production DbContext registration
            services.RemoveAll(typeof(DbContextOptions<DBContext>));

            // 2. Create a NEW internal service provider for EF Core
            // This is the key to preventing the provider conflict
            var efInternalServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // 3. Register the DbContext using the isolated provider
            services.AddDbContext<DBContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
                options.UseInternalServiceProvider(efInternalServiceProvider);
            });
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

        var city2 = new City
        {
            Id = 20,
            Name = "Cali",
            DepartmentId = 20,
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

        var deparment2 = new Department
        {
            Id = 10,
            Name = "Amazonas",
            NaturalAreas = new List<NaturalArea>(),
            CityCapitalId = 1,
            CityCapital = city2,
            Region = region,
            RegionId = region.Id,
            Cities = new List<City> {city2}
        };

        region.Departments = new List<Department> { deparment1, deparment2 };
        city1.Department = deparment1;
        city2.Department = deparment2;

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

            dbContext.Add(new TouristAttraction
            {
                Id = 2,
                Name = "Pueblito paisa",
                Description = "Lugar emblematico de Medellin",
                CityId = city1.Id,
                City = city1,
                Images = new string[] { "https://example.com/image1.jpg", "https://example.com/image2.jpg" },
            });

            dbContext.Add(new TouristAttraction
            {
                Id = 3,
                Name = "Parque Norte",
                Description = "Parque temático de ciencia y tecnología",
                CityId = city1.Id,
                City = city1,
                Images = new string[] { "https://example.com/image1.jpg", "https://example.com/image2.jpg" },
            });
        }
 
        if(!dbContext.ConstitutionArticles.Any())
        {
            dbContext.Add(constitutionArticle);

            dbContext.Add(new ConstitutionArticle
            {
                Id = 2,
                TitleNumber = 2,
                Title = "DE LA RAMA LEGISLATIVA",
                ChapterNumber = 1,
                Chapter = "DE LA FUNCION LEGISLATIVA",
                ArticleNumber = 2,
                Content = "La ley señalará las funciones que el Presidente de la República podrá elegir."
            });
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

        if(!dbContext.IndigenousReservations.Any())
        {
            dbContext.Add(new IndigenousReservation
            {
                Id = 1,
                Name = "Emberá",
                CityId = city1.Id,
                City = city1,
                Department = deparment1,
                DeparmentId = deparment1.Id,
            });

            dbContext.Add(new IndigenousReservation
            {
                Id = 2,
                Name = "Wayuu",
                CityId = city1.Id,
                City = city1,
                Department = deparment1,
                DeparmentId = deparment1.Id,
            });

            dbContext.Add(new IndigenousReservation
            {
                Id = 3,
                Name = "Zenú",
                CityId = city1.Id,
                City = city1,
                Department = deparment1,
                DeparmentId = deparment1.Id,
            });
        }

        if(!dbContext.InvasiveSpecies.Any())
        {
            dbContext.Add(new InvasiveSpecie
            {
                Id = 1,
                Name = "Sample Specie",
                ScientificName = "Sample scientific name",
                CommonNames = "Common name 1, Common name 2",
                Impact = "Impact description",
                RiskLevel = RiskLevel.High,
                Manage = "Manage description",
                UrlImage = "https://example.com/image.jpg",
            });

            dbContext.Add(new InvasiveSpecie
            {
                Id = 2,
                Name = "Another Specie",
                ScientificName = "Another scientific name",
                CommonNames = "Common name 1, Common name 2",
                Impact = "Impact description",
                RiskLevel = RiskLevel.High,
                Manage = "Manage description",
                UrlImage = "https://example.com/image.jpg",
            });

            dbContext.Add(new InvasiveSpecie
            {
                Id = 3,
                Name = "Third Specie",
                ScientificName = "Third scientific name",
                CommonNames = "Common name 1, Common name 2",
                Impact = "Impact description",
                RiskLevel = RiskLevel.High,
                Manage = "Manage description",
                UrlImage = "https://example.com/image.jpg",
            });
        }

        if(!dbContext.NativeCommunities.Any())
        {
            dbContext.Add(new NativeCommunity
            {
                Id = 1,
                Name = "Sample Community",
                Description = "Sample description",
                Languages = "Spanish, English",
                Images = new string[] { "https://example.com/image1.jpg", "https://example.com/image2.jpg" },
            });

            dbContext.Add(new NativeCommunity
            {
                Id = 2,
                Name = "Another Community",
                Description = "Another description",
                Languages = "Spanish, English",
                Images = new string[] { "https://example.com/image1.jpg", "https://example.com/image2.jpg" },
            });

            dbContext.Add(new NativeCommunity
            {
                Id = 3,
                Name = "Third Community",
                Description = "Third description",
                Languages = "Spanish, English",
                Images = new string[] { "https://example.com/image1.jpg", "https://example.com/image2.jpg" },
            });
        }

        if(!dbContext.Presidents.Any())
        {
            dbContext.Add(new President
            {
                Id = 1,
                Name = "Rafael",
                LastName = "Núñez",
                Description = "First in Colombia",
                Image = "https://example.com/image.jpg",
                PoliticalParty = "Sample Party",
                StartPeriodDate = new DateOnly(1865, 1, 1),
                EndPeriodDate = new DateOnly(1866, 1, 1),
                CityId = city1.Id,
                City = city1,
            });

            dbContext.Add(new President
            {
                Id = 2,
                Name = "Another President",
                LastName = "Another",
                StartPeriodDate = new DateOnly(1866, 1, 1),
                PoliticalParty = "Sample Party",
                Description = "President of Colombia",
                Image = "https://example.com/image.jpg",
                CityId = city1.Id,
                City = city1,
            });

            dbContext.Add(new President
            {
                Id = 3,
                Name = "Third President",
                LastName = "Third",
                StartPeriodDate = new DateOnly(1867, 1, 1),
                EndPeriodDate = new DateOnly(1868, 1, 1),
                PoliticalParty = "Sample Party",
                Description = "President of Colombia",
                Image = "https://example.com/image.jpg",
                CityId = city1.Id,
                City = city1,
            });
        }

        if(!dbContext.Radios.Any())
        {
            dbContext.Add(new Radio
            {
                Id = 1,
                Name = "Sample Radio",
                CityId = city1.Id,
                City = city1,
                Frequency = 101.1,
                Band = "FM",
                Url = new Uri("https://example.com/radio"),
                Streamers = new string[] { "Streamer1", "Streamer2" },
            });

            dbContext.Add(new Radio
            {
                Id = 2,
                Name = "Another Radio",
                CityId = city1.Id,
                City = city1,
                Frequency = 102.2,
                Band = "AM",
                Url = new Uri("https://example.com/radio"),
                Streamers = new string[] { "Streamer1", "Streamer2" },
            });

            dbContext.Add(new Radio
            {
                Id = 3,
                Name = "Third Radio",
                CityId = city1.Id,
                City = city1,
                Frequency = 103.3,
                Band = "AM",
                Url = new Uri("https://example.com/radio"),
                Streamers = new string[] { "Streamer1", "Streamer2" },
            });
        }

        if(!dbContext.TraditionalFairAndFestival.Any())
        {
            dbContext.Add(new TraditionalFairAndFestival
            {
                Id = 1,
                Name = "Sample Festival",
                Description = "Sample description",
                CityId = city1.Id,
                City = city1,
                Month = "January",
            });

            dbContext.Add(new TraditionalFairAndFestival
            {
                Id = 2,
                Name = "Another Festival",
                Description = "Another description",
                CityId = city1.Id,
                City = city1,
                Month = "February",
            });
            dbContext.Add(new TraditionalFairAndFestival
            {
                Id = 3,
                Name = "Super Event",
                Description = "Third description",
                CityId = 0,
                City = null!,
                Month = "March",
            });
        }

        if(!dbContext.TypicalDishes.Any())
        {
            dbContext.Add(new TypicalDish
            {
                Id = 1,
                Name = "Bandeja Paisa",
                Description = "Traditional dish from Antioquia",
                Ingredients = "Rice, beans, meat, avocado, plantain",
                DepartmentId = deparment1.Id,
                Department = deparment1,
                ImageUrl = "https://example.com/bandeja_paisa.jpg",
            });

            dbContext.Add(new TypicalDish
            {
                Id = 2,
                Name = "Arepa",
                Description = "Corn cake",
                Ingredients = "Corn flour, water, salt",
                DepartmentId = deparment1.Id,
                Department = deparment1,
                ImageUrl = "https://example.com/arepa.jpg",
            });

            dbContext.Add(new TypicalDish
            {
                Id = 3,
                Name = "Sancocho",
                Description = "Traditional soup",
                Ingredients = "Meat, plantain, yucca, corn",
                DepartmentId = deparment2.Id,
                Department = deparment2,
                ImageUrl = "https://example.com/sancocho.jpg",
            });
        }

        if(!dbContext.IntangibleHeritages.Any())
        {
            dbContext.Add(new IntangibleHeritage
            {
                Id = 1,
                Name = "Carnaval de Barranquilla",
                DepartmentId = deparment1.Id,
                Department = deparment1,
                Scope = "Nacional",
                InclusionYear = 2003,
            });

            dbContext.Add(new IntangibleHeritage
            {
                Id = 2,
                Name = "Fiesta de las Flores",
                DepartmentId = deparment1.Id,
                Department = deparment1,
                Scope = "Regional",
                InclusionYear = 2010,
            });

            dbContext.Add(new IntangibleHeritage
            {
                Id = 3,
                Name = "Semana Santa",
                DepartmentId = deparment2.Id,
                Department = deparment2,
                Scope = "Nacional",
                InclusionYear = 2012,
            });
        }

        if (!dbContext.HeritageCities.Any())
        {
            dbContext.Add(new HeritageCity
            {
                Id = 1,
                Name = "Cartagena",
                Description = "Historic walled city with colonial architecture.",
                CityId = city1.Id,
                City = city1,
                DepartmentId = deparment1.Id,
                Department = deparment1,
                Image = "https://example.com/cartagena.jpg",
            });

            dbContext.Add(new HeritageCity
            {
                Id = 2,
                Name = "Popayan",
                Description = "Colonial city known for its white architecture.",
                CityId = city1.Id,
                City = city1,
                DepartmentId = deparment1.Id,
                Department = deparment1,
                Image = "https://example.com/popayan.jpg",
            });

            dbContext.Add(new HeritageCity
            {
                Id = 3,
                Name = "Santa Cruz de Mompox",
                Description = "River town with preserved colonial heritage.",
                CityId = city1.Id,
                City = city1,
                DepartmentId = deparment1.Id,
                Department = deparment1,
                Image = "https://example.com/mompox.jpg",
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
            var fixture = new Fixture();
 
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customize<DateOnly>(composer => composer.FromFactory(() => DateOnly.FromDateTime(DateTime.Now)));

            SeedDatabase(dbContext);
        }
    }
}

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.EntityFrameworkCore;
using System.Linq;
using api;
using AutoFixture;
using api.Models;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
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
                options.UseInMemoryDatabase("TestColombiaDb");
            });
  
            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();
                dbContext.Database.EnsureDeleted();   
                dbContext.Database.EnsureCreated();   

                var fixture = new Fixture();
 
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
                fixture.Customize<DateOnly>(composer => composer.FromFactory(() => DateOnly.FromDateTime(DateTime.Now)));
 
                // CategoryNaturalAreas Registers 
                if (!dbContext.CategoryNaturalAreas.Any())
                { 
                   
                   dbContext.Add(new CategoryNaturalArea
                    {
                        Id = 1,
                        Name = "Área Natural Única",
                        Description = "Área geográfica que, por poseer condiciones especiales de flora o gea es un escenario natural raro.",
                        NaturalAreas = new List<NaturalArea>  
                        {
                            new NaturalArea
                            {
                                Id = 1,  
                                Name = "Área Natural Rara", 
                                CategoryNaturalAreaId = 1,  
                                DepartmentId = 101,  
                                DaneCode = 12345,  
                                LandArea = 5000,  
                                MaritimeArea = 0,  
                                Department = null
                            }
                        }
                    });


                    dbContext.Add(fixture.Build<CategoryNaturalArea>().With(p => p.Id ,2).Create());  
                    dbContext.Add(fixture.Build<CategoryNaturalArea>().With(p => p.Id ,3).Create());  
                    dbContext.Add(fixture.Build<CategoryNaturalArea>().With(p => p.Id ,4).Create());  
                    dbContext.Add(fixture.Build<CategoryNaturalArea>().With(p => p.Id ,5).Create());  
                    dbContext.SaveChanges();
                }

                 if (!dbContext.Airports.Any())
                { 
                   
                   dbContext.Add(new Airport
                    {
                        Id = 1,  
                        Name = "Base Aérea BG. Arturo Lema Posada",
                        IataCode = "N/A",
                        OaciCode = "N/A",
                        Type = "Militar",
                        CityId = 91,
                        Latitude = -75.42037792,
                        Longitude = 6.166336066,
                        Department = null,  
                        City = null  
                    });

                    dbContext.Add(fixture.Build<Airport>().With(p => p.Id ,2).Create());  
                    dbContext.Add(fixture.Build<Airport>().With(p => p.Id ,3).Create());  
                    dbContext.Add(fixture.Build<Airport>().With(p => p.Id ,4).Create());  
                    dbContext.Add(fixture.Build<Airport>().With(p => p.Id ,5).Create());  
               //     dbContext.SaveChanges();
                }


            // if (!dbContext.Airports.Any())
            // {
            //     dbContext.Airports.Add(new Airport
            //     {
            //         Id = 1,  
            //         Name = "Base Aérea BG. Arturo Lema Posada",
            //         IataCode = "N/A",
            //         OaciCode = "N/A",
            //         Type = "Militar",
            //         CityId = 91,
            //         Latitude = -75.42037792,
            //         Longitude = 6.166336066,
            //         Department = null,  
            //         City = null  
            //     });

             
            //     int idCounter = 2; 
            //     var additionalAirports = fixture.CreateMany<Airport>(4).Select(airport =>
            //     {
            //         airport.Id = idCounter++;  
            //         airport.Department = null;  
            //         airport.City = null;
            //         return airport;
            //     }).ToList();

            //     dbContext.Airports.AddRange(additionalAirports);
            // }
 
               


                    Console.WriteLine($"LEO-dbContext CategoryNaturalAreas: {dbContext.CategoryNaturalAreas.Count()}");
                    Console.WriteLine($"LEO-dbContext Airports: {dbContext.Airports.Count()}");  


            }
        });
    }
}


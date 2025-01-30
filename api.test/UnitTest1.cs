namespace api.test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        [Fact]
    public async Task GetAirports_ReturnsOk_WhenValidSortingProvided()
    {
        // Arrange
        // var airports = new List<Airport>
        // {
        //     new Airport { Id = 1, Name = "Airport A" },
        //     new Airport { Id = 2, Name = "Airport B" }
        // }.AsQueryable();

        // var mockSet = new Mock<DbSet<Airport>>();
        // mockSet.As<IQueryable<Airport>>().Setup(m => m.Provider).Returns(airports.Provider);
        // mockSet.As<IQueryable<Airport>>().Setup(m => m.Expression).Returns(airports.Expression);
        // mockSet.As<IQueryable<Airport>>().Setup(m => m.ElementType).Returns(airports.ElementType);
        // mockSet.As<IQueryable<Airport>>().Setup(m => m.GetEnumerator()).Returns(airports.GetEnumerator());

        // _mockDbContext.Setup(db => db.Airports).Returns(mockSet.Object);

        // var sortBy = "Name";
        // var sortDirection = "asc";

        // // Act
        // var result = await AirportsEndpoint.GetAirports(_mockDbContext.Object, sortBy, sortDirection);

        // // Assert
        // var okResult = Assert.IsType<Ok<List<Airport>>>(result);
        // Assert.NotNull(okResult.Value);
        // Assert.Equal(2, okResult.Value.Count);
        // Assert.Equal("Airport A", okResult.Value[0].Name);  
    }


    }
}
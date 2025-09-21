using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PeopleNetCoreBackend.Data;
using PeopleNetCoreBackend.Models;
using System.Net;
using System.Text.Json;
using Xunit;

namespace PeopleNetCoreBackend.Tests.Controllers
{
    public class PeopleControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public PeopleControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<PeopleDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add In-Memory database for testing
                    services.AddDbContext<PeopleDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetPeople_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/people");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetPeople_ReturnsJsonContent()
        {
            // Act
            var response = await _client.GetAsync("/api/people");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotNull(content);
            Assert.NotEmpty(content);
            
            // Verify it's valid JSON
            var jsonDocument = JsonDocument.Parse(content);
            Assert.NotNull(jsonDocument);
        }

        [Fact]
        public async Task GetPeople_ReturnsListOfPeople()
        {
            // Act
            var response = await _client.GetAsync("/api/people");
            var content = await response.Content.ReadAsStringAsync();
            var people = JsonSerializer.Deserialize<List<Person>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            Assert.NotNull(people);
            Assert.Equal(30, people.Count);
        }

        [Fact]
        public async Task GetPeople_ReturnsPeopleWithRequiredProperties()
        {
            // Act
            var response = await _client.GetAsync("/api/people");
            var content = await response.Content.ReadAsStringAsync();
            var people = JsonSerializer.Deserialize<List<Person>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            Assert.NotNull(people);
            Assert.NotEmpty(people);

            var firstPerson = people.First();
            Assert.NotNull(firstPerson.Cpf);
            Assert.NotNull(firstPerson.Name);
            Assert.NotNull(firstPerson.Genre);
            Assert.NotNull(firstPerson.Address);
            Assert.NotNull(firstPerson.Neighborhood);
            Assert.NotNull(firstPerson.State);
            Assert.True(firstPerson.Age > 0);
        }

        [Fact]
        public async Task GetPeople_ReturnsUniqueCpfs()
        {
            // Act
            var response = await _client.GetAsync("/api/people");
            var content = await response.Content.ReadAsStringAsync();
            var people = JsonSerializer.Deserialize<List<Person>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            Assert.NotNull(people);
            var cpfs = people.Select(p => p.Cpf).ToList();
            Assert.Equal(30, cpfs.Count);
            Assert.Equal(30, cpfs.Distinct().Count()); // All CPFs should be unique
        }
    }
}

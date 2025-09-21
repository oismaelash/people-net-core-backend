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

                    // Add In-Memory database for testing with unique name
                    var dbName = Guid.NewGuid().ToString();
                    services.AddDbContext<PeopleDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
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
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            Assert.NotNull(pagedResult);
            Assert.NotNull(pagedResult.Data);
            Assert.Equal(10, pagedResult.Data.Count()); // Default page size is 10
            Assert.True(pagedResult.TotalCount >= 10); // At least 10 people (seeded data)
            Assert.True(pagedResult.TotalPages >= 1);
            Assert.Equal(1, pagedResult.Page);
            Assert.Equal(10, pagedResult.PageSize);
        }

        [Fact]
        public async Task GetPeople_ReturnsPeopleWithRequiredProperties()
        {
            // Act
            var response = await _client.GetAsync("/api/people");
            var content = await response.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            Assert.NotNull(pagedResult);
            Assert.NotNull(pagedResult.Data);
            Assert.NotEmpty(pagedResult.Data);

            var firstPerson = pagedResult.Data.First();
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
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            Assert.NotNull(pagedResult);
            Assert.NotNull(pagedResult.Data);
            var cpfs = pagedResult.Data.Select(p => p.Cpf).ToList();
            Assert.Equal(10, cpfs.Count); // Default page size
            Assert.Equal(10, cpfs.Distinct().Count()); // All CPFs in this page should be unique
        }

        // Pagination tests
        [Fact]
        public async Task GetPeople_WithPageParameter_ReturnsCorrectPage()
        {
            // Act
            var response = await _client.GetAsync("/api/people?page=2&pageSize=5");
            var content = await response.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            Assert.NotNull(pagedResult);
            Assert.Equal(2, pagedResult.Page);
            Assert.Equal(5, pagedResult.PageSize);
            Assert.True(pagedResult.TotalCount >= 10); // At least 10 people (seeded data)
            Assert.True(pagedResult.TotalPages >= 2);
            Assert.True(pagedResult.HasPrevious);
            Assert.Equal(5, pagedResult.Data.Count());
        }

        [Fact]
        public async Task GetPeople_WithInvalidPage_ReturnsBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/api/people?page=0");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetPeople_WithInvalidPageSize_ReturnsBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/api/people?pageSize=101");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetPeople_WithNegativePageSize_ReturnsBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/api/people?pageSize=-1");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetPeople_LastPage_ShowsCorrectHasNext()
        {
            // First, get the total count to determine the last page
            var firstResponse = await _client.GetAsync("/api/people?page=1&pageSize=10");
            var firstContent = await firstResponse.Content.ReadAsStringAsync();
            var firstResult = JsonSerializer.Deserialize<PagedResult<Person>>(firstContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var lastPage = firstResult.TotalPages;

            // Act - get the last page
            var response = await _client.GetAsync($"/api/people?page={lastPage}&pageSize=10");
            var content = await response.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            Assert.NotNull(pagedResult);
            Assert.Equal(lastPage, pagedResult.Page);
            Assert.False(pagedResult.HasNext);
            if (lastPage > 1)
            {
                Assert.True(pagedResult.HasPrevious);
            }
        }

        // GET by CPF tests
        [Fact]
        public async Task GetPerson_WithValidCpf_ReturnsPerson()
        {
            // First, get a valid CPF from the list
            var peopleResponse = await _client.GetAsync("/api/people?page=1&pageSize=1");
            var peopleContent = await peopleResponse.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(peopleContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            var validCpf = pagedResult.Data.First().Cpf;

            // Act
            var response = await _client.GetAsync($"/api/people/{validCpf}");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var person = JsonSerializer.Deserialize<Person>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(person);
            Assert.Equal(validCpf, person.Cpf);
            Assert.NotEmpty(person.Name);
        }

        [Fact]
        public async Task GetPerson_WithInvalidCpf_ReturnsNotFound()
        {
            // Act - use a CPF that definitely doesn't exist
            var response = await _client.GetAsync("/api/people/99999999999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // POST tests
        [Fact]
        public async Task CreatePerson_WithValidData_ReturnsCreated()
        {
            // Arrange
            var newPerson = new Person
            {
                Cpf = "99999999999",
                Name = "Test Person",
                Genre = "Masculino",
                Address = "Test Address, 123",
                Age = 25,
                Neighborhood = "Test Neighborhood",
                State = "Test State"
            };

            var json = JsonSerializer.Serialize(newPerson);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/people", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var createdPerson = JsonSerializer.Deserialize<Person>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(createdPerson);
            Assert.Equal(newPerson.Cpf, createdPerson.Cpf);
            Assert.Equal(newPerson.Name, createdPerson.Name);
        }

        [Fact]
        public async Task CreatePerson_WithExistingCpf_ReturnsBadRequest()
        {
            // First, get an existing CPF from the list
            var peopleResponse = await _client.GetAsync("/api/people?page=1&pageSize=1");
            var peopleContent = await peopleResponse.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(peopleContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            var existingCpf = pagedResult.Data.First().Cpf;

            // Arrange
            var existingPerson = new Person
            {
                Cpf = existingCpf, // This CPF already exists in seeded data
                Name = "Test Person",
                Genre = "Masculino",
                Address = "Test Address, 123",
                Age = 25,
                Neighborhood = "Test Neighborhood",
                State = "Test State"
            };

            var json = JsonSerializer.Serialize(existingPerson);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/people", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreatePerson_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var invalidPerson = new Person
            {
                Cpf = "", // Invalid - empty CPF
                Name = "", // Invalid - empty name
                Genre = "",
                Address = "",
                Age = -1, // Invalid - negative age
                Neighborhood = "",
                State = ""
            };

            var json = JsonSerializer.Serialize(invalidPerson);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/people", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // PUT tests
        [Fact]
        public async Task UpdatePerson_WithValidData_ReturnsOk()
        {
            // First, get a valid CPF from the list
            var peopleResponse = await _client.GetAsync("/api/people?page=1&pageSize=1");
            var peopleContent = await peopleResponse.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(peopleContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            var validCpf = pagedResult.Data.First().Cpf;

            // Arrange
            var updatedPerson = new Person
            {
                Cpf = validCpf,
                Name = "Updated Name",
                Genre = "Masculino",
                Address = "Updated Address, 123",
                Age = 31,
                Neighborhood = "Updated Neighborhood",
                State = "Updated State"
            };

            var json = JsonSerializer.Serialize(updatedPerson);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/people/{validCpf}", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultPerson = JsonSerializer.Deserialize<Person>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(resultPerson);
            Assert.Equal("Updated Name", resultPerson.Name);
            Assert.Equal(31, resultPerson.Age);
        }

        [Fact]
        public async Task UpdatePerson_WithCpfMismatch_ReturnsBadRequest()
        {
            // First, get a valid CPF from the list
            var peopleResponse = await _client.GetAsync("/api/people?page=1&pageSize=1");
            var peopleContent = await peopleResponse.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(peopleContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            var validCpf = pagedResult.Data.First().Cpf;

            // Arrange
            var person = new Person
            {
                Cpf = "99999999999", // Different from URL parameter
                Name = "Test Person",
                Genre = "Masculino",
                Address = "Test Address, 123",
                Age = 25,
                Neighborhood = "Test Neighborhood",
                State = "Test State"
            };

            var json = JsonSerializer.Serialize(person);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/people/{validCpf}", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePerson_WithNonExistentCpf_ReturnsNotFound()
        {
            // Arrange
            var person = new Person
            {
                Cpf = "99999999999",
                Name = "Test Person",
                Genre = "Masculino",
                Address = "Test Address, 123",
                Age = 25,
                Neighborhood = "Test Neighborhood",
                State = "Test State"
            };

            var json = JsonSerializer.Serialize(person);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/people/99999999999", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // DELETE tests
        [Fact]
        public async Task DeletePerson_WithValidCpf_ReturnsNoContent()
        {
            // First, get a valid CPF from the list
            var peopleResponse = await _client.GetAsync("/api/people?page=1&pageSize=1");
            var peopleContent = await peopleResponse.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(peopleContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            var validCpf = pagedResult.Data.First().Cpf;

            // Act
            var response = await _client.DeleteAsync($"/api/people/{validCpf}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeletePerson_WithNonExistentCpf_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync("/api/people/99999999999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeletePerson_ThenGetPerson_ReturnsNotFound()
        {
            // First, get a valid CPF from the list
            var peopleResponse = await _client.GetAsync("/api/people?page=1&pageSize=1");
            var peopleContent = await peopleResponse.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<Person>>(peopleContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            var cpfToDelete = pagedResult.Data.First().Cpf;

            // Act - Delete the person
            var deleteResponse = await _client.DeleteAsync($"/api/people/{cpfToDelete}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Act - Try to get the deleted person
            var getResponse = await _client.GetAsync($"/api/people/{cpfToDelete}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}

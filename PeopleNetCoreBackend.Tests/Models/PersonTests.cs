using PeopleNetCoreBackend.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace PeopleNetCoreBackend.Tests.Models
{
    public class PersonTests
    {
        [Fact]
        public void Person_ShouldHaveRequiredProperties()
        {
            // Arrange & Act
            var person = new Person
            {
                Cpf = "12345678901",
                Name = "João Silva",
                Genre = "Masculino",
                Address = "Rua das Flores, 123",
                Age = 30,
                Neighborhood = "Centro",
                State = "São Paulo"
            };

            // Assert
            Assert.Equal("12345678901", person.Cpf);
            Assert.Equal("João Silva", person.Name);
            Assert.Equal("Masculino", person.Genre);
            Assert.Equal("Rua das Flores, 123", person.Address);
            Assert.Equal(30, person.Age);
            Assert.Equal("Centro", person.Neighborhood);
            Assert.Equal("São Paulo", person.State);
        }

        [Fact]
        public void Person_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var person = new Person();

            // Assert
            Assert.Equal(string.Empty, person.Cpf);
            Assert.Equal(string.Empty, person.Name);
            Assert.Equal(string.Empty, person.Genre);
            Assert.Equal(string.Empty, person.Address);
            Assert.Equal(0, person.Age);
            Assert.Equal(string.Empty, person.Neighborhood);
            Assert.Equal(string.Empty, person.State);
        }

        [Theory]
        [InlineData("12345678901", "João Silva", "Masculino", "Rua das Flores, 123", 30, "Centro", "São Paulo")]
        [InlineData("98765432100", "Maria Santos", "Feminino", "Avenida Paulista, 456", 25, "Bela Vista", "São Paulo")]
        public void Person_ShouldAcceptValidData(string cpf, string name, string genre, string address, int age, string neighborhood, string state)
        {
            // Arrange & Act
            var person = new Person
            {
                Cpf = cpf,
                Name = name,
                Genre = genre,
                Address = address,
                Age = age,
                Neighborhood = neighborhood,
                State = state
            };

            // Assert
            Assert.Equal(cpf, person.Cpf);
            Assert.Equal(name, person.Name);
            Assert.Equal(genre, person.Genre);
            Assert.Equal(address, person.Address);
            Assert.Equal(age, person.Age);
            Assert.Equal(neighborhood, person.Neighborhood);
            Assert.Equal(state, person.State);
        }
    }
}

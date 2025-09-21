using Microsoft.EntityFrameworkCore;
using PeopleNetCoreBackend.Data;
using PeopleNetCoreBackend.Models;
using Xunit;

namespace PeopleNetCoreBackend.Tests.Data
{
    public class PeopleDbContextTests
    {
        [Fact]
        public void PeopleDbContext_ShouldBeCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PeopleDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            // Act
            using var context = new PeopleDbContext(options);

            // Assert
            Assert.NotNull(context);
            Assert.NotNull(context.People);
        }

        [Fact]
        public void PeopleDbContext_ShouldHaveSeededData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PeopleDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDbSeeded")
                .Options;

            // Act
            using var context = new PeopleDbContext(options);
            context.Database.EnsureCreated();

            var people = context.People.ToList();

            // Assert
            Assert.Equal(30, people.Count);
        }

        [Fact]
        public void PeopleDbContext_ShouldHaveValidPersonData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PeopleDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDbValidData")
                .Options;

            // Act
            using var context = new PeopleDbContext(options);
            context.Database.EnsureCreated();

            var people = context.People.ToList();
            var firstPerson = people.First();

            // Assert
            Assert.NotNull(firstPerson.Cpf);
            Assert.NotEmpty(firstPerson.Cpf);
            Assert.NotNull(firstPerson.Name);
            Assert.NotEmpty(firstPerson.Name);
            Assert.NotNull(firstPerson.Genre);
            Assert.NotEmpty(firstPerson.Genre);
            Assert.NotNull(firstPerson.Address);
            Assert.NotEmpty(firstPerson.Address);
            Assert.True(firstPerson.Age > 0);
            Assert.NotNull(firstPerson.Neighborhood);
            Assert.NotEmpty(firstPerson.Neighborhood);
            Assert.NotNull(firstPerson.State);
            Assert.NotEmpty(firstPerson.State);
        }

        [Fact]
        public void PeopleDbContext_ShouldHaveUniqueCpfs()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PeopleDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDbUniqueCpfs")
                .Options;

            // Act
            using var context = new PeopleDbContext(options);
            context.Database.EnsureCreated();

            var people = context.People.ToList();
            var cpfs = people.Select(p => p.Cpf).ToList();

            // Assert
            Assert.Equal(30, cpfs.Count);
            Assert.Equal(30, cpfs.Distinct().Count());
        }
    }
}

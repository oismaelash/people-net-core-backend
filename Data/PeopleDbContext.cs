using Microsoft.EntityFrameworkCore;
using PeopleNetCoreBackend.Models;

namespace PeopleNetCoreBackend.Data
{
    public class PeopleDbContext : DbContext
    {
        public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Person entity
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Cpf);
                entity.Property(e => e.Cpf).IsRequired().HasMaxLength(11);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Genre).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Neighborhood).IsRequired().HasMaxLength(100);
                entity.Property(e => e.State).IsRequired().HasMaxLength(50);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var people = new List<Person>
            {
                new Person { Cpf = "12345678901", Name = "João Silva", Genre = "Masculino", Address = "Rua das Flores, 123", Age = 30, Neighborhood = "Centro", State = "São Paulo" },
                new Person { Cpf = "12345678902", Name = "Maria Santos", Genre = "Feminino", Address = "Avenida Paulista, 456", Age = 25, Neighborhood = "Bela Vista", State = "Rio de Janeiro" },
                new Person { Cpf = "12345678903", Name = "Pedro Oliveira", Genre = "Masculino", Address = "Rua Augusta, 789", Age = 35, Neighborhood = "Consolação", State = "Minas Gerais" },
                new Person { Cpf = "12345678904", Name = "Ana Costa", Genre = "Feminino", Address = "Rua Oscar Freire, 321", Age = 28, Neighborhood = "Jardins", State = "Bahia" },
                new Person { Cpf = "12345678905", Name = "Carlos Ferreira", Genre = "Masculino", Address = "Avenida Faria Lima, 654", Age = 42, Neighborhood = "Itaim Bibi", State = "Paraná" },
                new Person { Cpf = "12345678906", Name = "Lucia Almeida", Genre = "Feminino", Address = "Rua Haddock Lobo, 987", Age = 33, Neighborhood = "Cerqueira César", State = "Rio Grande do Sul" },
                new Person { Cpf = "12345678907", Name = "Roberto Lima", Genre = "Masculino", Address = "Avenida Rebouças, 147", Age = 29, Neighborhood = "Pinheiros", State = "Santa Catarina" },
                new Person { Cpf = "12345678908", Name = "Fernanda Rocha", Genre = "Feminino", Address = "Rua Teodoro Sampaio, 258", Age = 31, Neighborhood = "Pinheiros", State = "Pernambuco" },
                new Person { Cpf = "12345678909", Name = "Marcos Pereira", Genre = "Masculino", Address = "Avenida Brigadeiro Luiz Antonio, 369", Age = 27, Neighborhood = "Bela Vista", State = "Ceará" },
                new Person { Cpf = "12345678910", Name = "Juliana Martins", Genre = "Feminino", Address = "Rua da Consolação, 741", Age = 26, Neighborhood = "Consolação", State = "Goiás" },
                new Person { Cpf = "12345678911", Name = "Antonio Souza", Genre = "Masculino", Address = "Avenida 9 de Julho, 852", Age = 38, Neighborhood = "Bela Vista", State = "São Paulo" },
                new Person { Cpf = "12345678912", Name = "Patricia Gomes", Genre = "Feminino", Address = "Rua Bela Cintra, 963", Age = 24, Neighborhood = "Jardins", State = "Rio de Janeiro" },
                new Person { Cpf = "12345678913", Name = "Rafael Barbosa", Genre = "Masculino", Address = "Avenida Paulista, 159", Age = 36, Neighborhood = "Bela Vista", State = "Minas Gerais" },
                new Person { Cpf = "12345678914", Name = "Camila Dias", Genre = "Feminino", Address = "Rua Augusta, 357", Age = 29, Neighborhood = "Consolação", State = "Bahia" },
                new Person { Cpf = "12345678915", Name = "Diego Nascimento", Genre = "Masculino", Address = "Avenida Faria Lima, 468", Age = 32, Neighborhood = "Itaim Bibi", State = "Paraná" },
                new Person { Cpf = "12345678916", Name = "Beatriz Cardoso", Genre = "Feminino", Address = "Rua Oscar Freire, 579", Age = 27, Neighborhood = "Jardins", State = "Rio Grande do Sul" },
                new Person { Cpf = "12345678917", Name = "Gabriel Moreira", Genre = "Masculino", Address = "Avenida Rebouças, 680", Age = 34, Neighborhood = "Pinheiros", State = "Santa Catarina" },
                new Person { Cpf = "12345678918", Name = "Larissa Vieira", Genre = "Feminino", Address = "Rua Teodoro Sampaio, 791", Age = 25, Neighborhood = "Pinheiros", State = "Pernambuco" },
                new Person { Cpf = "12345678919", Name = "Thiago Correia", Genre = "Masculino", Address = "Avenida Brigadeiro Luiz Antonio, 802", Age = 30, Neighborhood = "Bela Vista", State = "Ceará" },
                new Person { Cpf = "12345678920", Name = "Mariana Lopes", Genre = "Feminino", Address = "Rua da Consolação, 913", Age = 28, Neighborhood = "Consolação", State = "Goiás" },
                new Person { Cpf = "12345678921", Name = "Felipe Ribeiro", Genre = "Masculino", Address = "Avenida 9 de Julho, 124", Age = 33, Neighborhood = "Bela Vista", State = "São Paulo" },
                new Person { Cpf = "12345678922", Name = "Isabela Cunha", Genre = "Feminino", Address = "Rua Bela Cintra, 235", Age = 26, Neighborhood = "Jardins", State = "Rio de Janeiro" },
                new Person { Cpf = "12345678923", Name = "Bruno Mendes", Genre = "Masculino", Address = "Avenida Paulista, 346", Age = 31, Neighborhood = "Bela Vista", State = "Minas Gerais" },
                new Person { Cpf = "12345678924", Name = "Natália Araújo", Genre = "Feminino", Address = "Rua Augusta, 457", Age = 29, Neighborhood = "Consolação", State = "Bahia" },
                new Person { Cpf = "12345678925", Name = "Vinicius Castro", Genre = "Masculino", Address = "Avenida Faria Lima, 568", Age = 35, Neighborhood = "Itaim Bibi", State = "Paraná" },
                new Person { Cpf = "12345678926", Name = "Amanda Freitas", Genre = "Feminino", Address = "Rua Oscar Freire, 679", Age = 24, Neighborhood = "Jardins", State = "Rio Grande do Sul" },
                new Person { Cpf = "12345678927", Name = "Leonardo Monteiro", Genre = "Masculino", Address = "Avenida Rebouças, 780", Age = 37, Neighborhood = "Pinheiros", State = "Santa Catarina" },
                new Person { Cpf = "12345678928", Name = "Carolina Nunes", Genre = "Feminino", Address = "Rua Teodoro Sampaio, 891", Age = 32, Neighborhood = "Pinheiros", State = "Pernambuco" },
                new Person { Cpf = "12345678929", Name = "Rodrigo Campos", Genre = "Masculino", Address = "Avenida Brigadeiro Luiz Antonio, 902", Age = 28, Neighborhood = "Bela Vista", State = "Ceará" },
                new Person { Cpf = "12345678930", Name = "Vanessa Andrade", Genre = "Feminino", Address = "Rua da Consolação, 113", Age = 30, Neighborhood = "Consolação", State = "Goiás" }
            };

            modelBuilder.Entity<Person>().HasData(people);
        }
    }
}

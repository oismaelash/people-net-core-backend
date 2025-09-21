using Microsoft.EntityFrameworkCore;
using PeopleNetCoreBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "People Management API",
        Version = "v1",
        Description = "A comprehensive API for managing people data",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ismael Ash",
            Email = "contato@ismaelnascimento.com",
            Url = new Uri("https://github.com/oismaelash")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Include XML comments for better documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Group controllers by their names
    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    c.DocInclusionPredicate((name, api) => true);
});

// Add Entity Framework with In-Memory database
builder.Services.AddDbContext<PeopleDbContext>(options =>
    options.UseInMemoryDatabase("PeopleDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger in all environments for easier testing
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "People Management API v1");
    c.RoutePrefix = "swagger"; // Set the route prefix for Swagger UI
    c.DocumentTitle = "People Management API Documentation";
    c.DefaultModelsExpandDepth(-1); // Hide the models section by default
    c.DisplayRequestDuration();
    c.EnableDeepLinking();
    c.EnableFilter();
    c.ShowExtensions();
    c.EnableValidator();
});

app.UseHttpsRedirection();

app.UseAuthorization();

// Redirect root path to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/health", () => Results.Ok("OK"));

app.MapControllers();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PeopleDbContext>();
    context.Database.EnsureCreated();
}

app.Run();

// Make Program class accessible for testing
public partial class Program { }

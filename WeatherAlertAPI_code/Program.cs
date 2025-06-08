using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WeatherAlertAPI.Data;
using WeatherAlertAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework com logging melhorado
builder.Services.AddDbContext<WeatherDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        // Usar SQLite com arquivo físico para desenvolvimento
        options.UseSqlite("Data Source=WeatherAlertAPI.db")
               .EnableSensitiveDataLogging()
               .LogTo(Console.WriteLine, LogLevel.Information);
    }
    else
    {
        // Usar Oracle em produção
        options.UseOracle(connectionString);
    }
});

// Register services
builder.Services.AddHttpClient<IOpenMeteoService, OpenMeteoService>();
builder.Services.AddScoped<IHeatRiskService, HeatRiskService>();
builder.Services.AddScoped<ITipsService, TipsService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Weather Alert API",
        Version = "v1",
        Description = "API para monitoramento de temperatura e alertas de calor extremo com dicas preventivas personalizadas"
       
    });
});

// Configure Razor Pages
builder.Services.AddRazorPages();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather Alert API v1");
        c.RoutePrefix = "swagger";
    });
}

// Configuração do banco de dados e seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var context = services.GetRequiredService<WeatherDbContext>();
        
        // Garante que o banco seja criado e as migrações aplicadas
        context.Database.EnsureCreated();
        
        // Verifica se as tabelas existem
        if (context.Database.CanConnect())
        {
            Console.WriteLine("Banco de dados conectado com sucesso.");
            
            // Aplica seed data apenas se não houver dados
            if (!context.Cidades.Any())
            {
                Console.WriteLine("Aplicando seed data...");
                SeedData.Initialize(context);
                Console.WriteLine("Seed data aplicado com sucesso.");
            }
            else
            {
                Console.WriteLine("Banco de dados já contém dados. Seed data não aplicado.");
            }
        }
        else
        {
            Console.WriteLine("Não foi possível conectar ao banco de dados.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao configurar o banco de dados");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

// Endpoints de teste
app.MapGet("/test-openmeteo", async (IOpenMeteoService openMeteoService) =>
{
    var forecast = await openMeteoService.GetWeatherForecastAsync(-15.7942m, -47.8822m);
    return forecast;
});

app.MapGet("/test-database", async (WeatherDbContext context) =>
{
    try
    {
        var cidades = await context.Cidades.ToListAsync();
        return Results.Ok(new { 
            DatabaseStatus = "Operacional",
            CidadesCount = cidades.Count 
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Erro ao acessar o banco de dados: {ex.Message}");
    }
});

app.Run();
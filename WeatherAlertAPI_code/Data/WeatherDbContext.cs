using Microsoft.EntityFrameworkCore;
using WeatherAlertAPI.Models;

namespace WeatherAlertAPI.Data
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
        {
        }

        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Dica> Dicas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais se necessário
            modelBuilder.Entity<Cidade>(entity =>
            {
                entity.Property(e => e.Latitude)
                    .HasPrecision(10, 6);
                
                entity.Property(e => e.Longitude)
                    .HasPrecision(10, 6);
            });
        }
    }
}


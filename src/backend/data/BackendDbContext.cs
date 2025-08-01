using data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace data
{
    public class BackendDbContext(DbContextOptions<BackendDbContext> options, IConfiguration configuration) : DbContext(options)
    {
        private readonly IConfiguration Configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = this.Configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        public override int SaveChanges()
            => base.SaveChanges();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}

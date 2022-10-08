using CSC.Domain;
using Microsoft.EntityFrameworkCore;

namespace CSC
{
    public class DatabaseContext : DbContext
    {
        private const string ConnectionString = "data source=localhost;initial catalog=CSC;integrated security=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                            => optionsBuilder.UseSqlServer(ConnectionString)
                                             .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        public DbSet<Country>? Countries { get; set; }
        public DbSet<State>? States { get; set; }
        public DbSet<City>? Cities { get; set; }
        public DbSet<Timezone>? Timezones { get; set; }
        public DbSet<Translation>? Translations { get; set; }
    }

}
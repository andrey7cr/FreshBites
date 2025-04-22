using FreshBites.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace FreshBites.Data
{
    public class FreshBitesDbContext : IdentityDbContext<Usuario, IdentityRole, string>
    {
        public class FreshBitesDbContextFactory : IDesignTimeDbContextFactory<FreshBitesDbContext>
        {
            public FreshBitesDbContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<FreshBitesDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectionString);

                return new FreshBitesDbContext(optionsBuilder.Options);
            }
        }

        public FreshBitesDbContext(DbContextOptions<FreshBitesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
    }

}

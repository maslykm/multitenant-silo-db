using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace App.Control
{
    public class ControlDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; } = null!;

        public ControlDbContext(DbContextOptions<ControlDbContext> options) : base(options)
        {
        }

        internal void Initialize()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(Database.GetConnectionString());

            string[] tenants = { "Tenant1", "Tenant2", "Tenant3" };
            foreach (var tenant in tenants)
            {
                builder.Database = tenant;
                Tenants.Add(new Tenant
                {
                    Name = builder.Database,
                    ConnectionString = builder.ConnectionString
                });
            }

            SaveChanges();
        }
    }
}

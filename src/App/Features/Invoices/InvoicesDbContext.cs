using Microsoft.EntityFrameworkCore;

namespace App.Features.Invoices
{
    internal class InvoicesDbContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; } = null!;

        public InvoicesDbContext(DbContextOptions<InvoicesDbContext> options) : base(options)
        {
        }
    }
}

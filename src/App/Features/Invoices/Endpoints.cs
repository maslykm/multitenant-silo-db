using App.Control;
using Microsoft.EntityFrameworkCore;

namespace App.Features.Invoices
{
    public static class Endpoints
    {
        public static void AddInvoicesFeature(this IServiceCollection services)
        {
            services.AddScoped<InvoicesListHandler>();
            services.AddScoped<InvoicesAddHandler>();
            services.AddDbContext<InvoicesDbContext>((sp, options) =>
            {
                var srv = sp.GetRequiredService<TenantControlService>();
                options.UseNpgsql(srv.GetTenantConnectionString());
                options.EnableSensitiveDataLogging();
            });
        }

        public static void UseInvoicesFeature(this WebApplication app)
        {
            app.MapGet("/invoices", (InvoicesListHandler handler) => handler.Handle());
            app.MapPost("/invoices", (InvoicesAddHandler handler) => handler.Handle());
        }
    }
}

using App.Control;
using App.Features.Invoices;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, config) =>
        {
            config
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console(theme: AnsiConsoleTheme.Literate);
        });

        builder.Services.AddOpenApi();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<TenantDetectMiddleware>();
        builder.Services.AddScoped<TenantControlService>();
        builder.Services.AddDbContext<ControlDbContext>(options => options
            .UseNpgsql(builder.Configuration.GetConnectionString("Control"))
            .EnableSensitiveDataLogging());

        builder.Services.AddInvoicesFeature();

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseMiddleware<TenantDetectMiddleware>();

        var scope = app.Services.CreateScope();
        var ctrlDb = scope.ServiceProvider.GetRequiredService<ControlDbContext>();
        ctrlDb.Initialize();

        ctrlDb.Tenants.ToList()
            .ForEach(t =>
            {
                var opt = new DbContextOptionsBuilder<InvoicesDbContext>()
                    .UseNpgsql(t.ConnectionString)
                    .UseLoggerFactory(scope.ServiceProvider.GetService<ILoggerFactory>())
                    .EnableSensitiveDataLogging();

                var db = new InvoicesDbContext(opt.Options);
                db.Database.EnsureCreated();
            });

        app.UseInvoicesFeature();

        app.Run();
    }
}

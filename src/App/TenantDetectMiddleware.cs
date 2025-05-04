using App.Control;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

internal class TenantDetectMiddleware : IMiddleware
{
    private readonly ControlDbContext _controlDb;

    public TenantDetectMiddleware(ControlDbContext controlDb)
    {
        _controlDb = controlDb;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.TryGetValue("X-Tenant-ID", out StringValues tenantId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Tenant ID is required.");
            return;
        }

        var tenant = await _controlDb.Tenants.FirstOrDefaultAsync(t => t.Name == tenantId.ToString());
        if (tenant == null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Tenant not found.");
            return;
        }

        context.Items["TenantId"] = tenant.Id;
        context.Items["TenantDb"] = tenant.ConnectionString;

        await next.Invoke(context);
    }
}
namespace App.Control
{
    internal class TenantControlService
    {
        private readonly IHttpContextAccessor _accessor;

        public TenantControlService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string GetTenantConnectionString()
        {
            if (_accessor.HttpContext!.Items.TryGetValue("TenantDb", out object? tenantDb))
            {
                return tenantDb.ToString();
            }

            throw new InvalidOperationException("Tenant connection string not found in context.");
        }

        public string GetTenantId()
        {
            if (_accessor.HttpContext!.Items.TryGetValue("TenantId", out object? tenantId))
            {
                return tenantId.ToString();
            }

            throw new InvalidOperationException("Tenant ID not found in context.");
        }
    }
}

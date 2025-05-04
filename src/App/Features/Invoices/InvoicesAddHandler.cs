using App.Control;

namespace App.Features.Invoices
{
    internal class InvoicesAddHandler
    {
        private readonly InvoicesDbContext _invoicesDbContext;
        private readonly TenantControlService _service;

        public InvoicesAddHandler(InvoicesDbContext invoicesDbContext, TenantControlService service)
        {
            _invoicesDbContext = invoicesDbContext;
            _service = service;
        }

        internal IResult Handle()
        {
            var invoice = Invoice.ForTenant(_service.GetTenantId());
            _invoicesDbContext.Invoices.Add(invoice);
            _invoicesDbContext.SaveChanges();
            return Results.Created($"/invoices/{invoice.Id}", invoice);
        }
    }
}

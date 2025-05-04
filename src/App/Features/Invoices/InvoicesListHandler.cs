namespace App.Features.Invoices
{
    internal class InvoicesListHandler
    {
        private readonly InvoicesDbContext _invoicesDbContext;

        public InvoicesListHandler(InvoicesDbContext invoicesDbContext)
        {
            _invoicesDbContext = invoicesDbContext;
        }

        internal IResult Handle()
        {
            List<Invoice> items = _invoicesDbContext.Invoices.ToList();
            return Results.Ok(items);
        }
    }
}

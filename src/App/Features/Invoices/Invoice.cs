namespace App.Features.Invoices
{
    internal class Invoice
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public static Invoice ForTenant(string tenant)
        {
            return new Invoice
            {
                Number = tenant + "-INV/" + DateTime.UtcNow.Ticks,
                Date = DateTime.UtcNow
            };
        }
    }
}

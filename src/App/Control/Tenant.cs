namespace App.Control
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }
}

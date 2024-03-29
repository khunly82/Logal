namespace Logal.Models
{
    public class FactureModel
    {
        public string Reference { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string VAT { get; set; } = null!;

        public List<Article> Articles { get; set; } = null!;
    }

    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice{ get; set; }
    }
}

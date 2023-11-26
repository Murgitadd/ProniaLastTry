namespace ProniaLastTry.Models
{
    public class ProductImage
    {
        public int id { get; set; }
        public string Image { get; set; }
        public string Alt { get; set; }
        public bool? IsPrimary { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
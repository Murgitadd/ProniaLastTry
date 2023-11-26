using ProniaLastTry.Models;

namespace ProniaLastTry.ModelsVM
{
    public class HomeVM
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Slide> Slides { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}

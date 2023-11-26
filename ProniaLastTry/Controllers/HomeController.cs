using ProniaLastTry.DAL;
using ProniaLastTry.Models;
using ProniaLastTry.ModelsVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProniaLastTry.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Product> products = _context.Products
            .Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).OrderByDescending(s => s.CountId).Take(8)
            .ToList();

            List<Slide> slides = _context.Slides.OrderBy(s => s.Id).Take(3).ToList();
            HomeVM vm = new HomeVM { Slides = slides, Products = products };

            return View(vm);
        }
    }
}

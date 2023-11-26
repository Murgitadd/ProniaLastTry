using ProniaLastTry.Areas.Admin.ViewModels;
using ProniaLastTry.DAL;
using ProniaLastTry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProniaLastTry.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        private readonly AppDbContext _context;

        public SizeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Size> colors = await _context.Sizes.Include(c => c.ProductSizes).ToListAsync();
            return View(colors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateSizeVM sizeVM)
        {
            if (!ModelState.IsValid)
            {
                return View(sizeVM);
            }

            bool result = await _context.Sizes.AnyAsync(c => c.Name.ToLower().Trim() == sizeVM.Name.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Color.Name", "This color is already exists!");
                return View(sizeVM);
            }
            Size size = new Size { Name = sizeVM.Name };

            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Size size = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);

            if (size == null) return NotFound();
            CreateUpdateSizeVM sizeVM = new CreateUpdateSizeVM { Name = size.Name };


            return View(sizeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateUpdateSizeVM sizeVM)
        {
            if (!ModelState.IsValid) return View();

            Size existed = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();

            bool result = await _context.Sizes.AnyAsync(c => c.Name.ToLower().Trim() == sizeVM.Name.ToLower().Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError("Name", "This size is already aviable!");
                return View();
            }

            existed.Name = sizeVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Size existed = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Sizes.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) return BadRequest();
            Size size = await _context.Sizes
                .Include(p => p.ProductSizes)
                .ThenInclude(p => p.Product).ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (size == null) return NotFound();

            return View(size);
        }
    }
}

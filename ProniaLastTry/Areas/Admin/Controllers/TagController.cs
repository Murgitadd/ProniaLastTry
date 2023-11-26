using ProniaLastTry.Areas.Admin.ViewModels;
using ProniaLastTry.DAL;
using ProniaLastTry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace ProniaLastTry.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Tag> colors = await _context.Tags.Include(c => c.ProductTags).ToListAsync();
            return View(colors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateTagVM tagVM)
        {
            if (!ModelState.IsValid)
            {
                return View(tagVM);
            }

            bool result = await _context.Tags.AnyAsync(c => c.Name.ToLower().Trim() == tagVM.Name.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Color.Name", "This color is already exists!");
                return View(tagVM);
            }
            Tag tag = new Tag { Name = tagVM.Name };

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Tag tag = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);

            if (tag == null) return NotFound();
            CreateUpdateTagVM tagVM = new CreateUpdateTagVM { Name = tag.Name };


            return View(tagVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateUpdateTagVM tagVM)
        {
            if (!ModelState.IsValid) return View(tagVM);

            Tag existed = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();

            bool result = await _context.Tags.AnyAsync(c => c.Name.ToLower().Trim() == tagVM.Name.ToLower().Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError("Name", "There is already such a tag!");
                return View(tagVM);
            }

            existed.Name = tagVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Tag existed = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Tags.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) return BadRequest();
            Tag tag = await _context.Tags
                .Include(p => p.ProductTags)
                .ThenInclude(p => p.Product).ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (tag == null) return NotFound();

            return View(tag);
        }
    }
}

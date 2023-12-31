﻿using ProniaLastTry.Areas.Admin.ViewModels;
using ProniaLastTry.DAL;
using ProniaLastTry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProniaLastTry.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ColorController : Controller
    {
        private readonly AppDbContext _context;

        public ColorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Color> colors = await _context.Colors.Include(c => c.ProductColors).ToListAsync();
            return View(colors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateCategoryVM colorVM)
        {
            if (!ModelState.IsValid)
            {
                return View(colorVM);
            }

            bool result = await _context.Colors.AnyAsync(c => c.Name.ToLower().Trim() == colorVM.Name.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Color.Name", "This color is already exists!");
                return View(colorVM);
            }
            Color color = new Color { Name = colorVM.Name };

            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Color color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);

            if (color == null) return NotFound();
            CreateUpdateColorVM colorVM = new CreateUpdateColorVM { Name = color.Name };


            return View(colorVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateUpdateColorVM colorVM)
        {
            if (!ModelState.IsValid) return View(colorVM);

            Color existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();

            bool result = await _context.Colors.AnyAsync(c => c.Name.ToLower().Trim() == colorVM.Name.ToLower().Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError("Name", "This color is already exists!");
                return View(colorVM);
            }

            existed.Name = colorVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Color existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Colors.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) return BadRequest();
            Color color = await _context.Colors
                .Include(p => p.ProductColors)
                .ThenInclude(p => p.Product).ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (color == null) return NotFound();

            return View(color);
        }
    }
}

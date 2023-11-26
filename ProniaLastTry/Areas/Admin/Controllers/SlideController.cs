using ProniaLastTry.Areas.Admin.ViewModels;
using ProniaLastTry.DAL;
using ProniaLastTry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaPrime.Utilities.Extensions;

namespace ProniaLastTry.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slide = await _context.Slides.ToListAsync();
            return View(slide);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            if(slideVM.Photo is null)
            {
                ModelState.AddModelError("Photo", "Please upload an image...");
                return View(slideVM);
            }

            if (!slideVM.Photo.ValidateType())
            {
                ModelState.AddModelError("Photo", "This file type is not supported!");
                return View(slideVM);
            }

            if(!slideVM.Photo.ValidateSize(10))
            {
                ModelState.AddModelError("Photo", "Max size of an image is 10 mb, Upgrade dicord nitro for unlimited uploads!");
                return View(slideVM);
            }

            

            string fileName = await slideVM.Photo.CreateFile(_env.WebRootPath, "assets","images", "website-images");

            Slide slide = new Slide
            {
                Title = slideVM.Title,
                SubTitle = slideVM.SubTitle,
                Description = slideVM.Description,
                Image = fileName
            };


            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);

            if (slide == null) return NotFound();

            UpdateSlideVM slideVM = new UpdateSlideVM 
            {
                Title= slide.Title,
                SubTitle = slide.SubTitle,
                Description = slide.Description,
                Image = slide.Image,

            };

            return View(slideVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSlideVM slideVM)
        {
            if (!ModelState.IsValid) return View(slideVM);

            Slide existed = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (slideVM.Photo is not null) 
            {

                if (!slideVM.Photo.ValidateType())
                {
                    ModelState.AddModelError("Photo", "This file type is not supported");
                    return View(existed);
                }

                if (!slideVM.Photo.ValidateSize(10))
                {
                    ModelState.AddModelError("Photo", "Max size of an image is 10 mb, Upgrade dicord nitro for unlimited uploads!");
                    return View(existed);
                }
                string newImage = await slideVM.Photo.CreateFile(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image = newImage;
            }

            existed.Title = slideVM.Title;
            existed.SubTitle = slideVM.SubTitle;
            existed.Description = slideVM.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slide existed = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);

            if (existed == null) return NotFound();

            existed.Image.DeleteFile(_env.WebRootPath, "assets","images", "website-images");

            _context.Slides.Remove(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);
            if (slide == null) return NotFound();

            return View(slide);
        }
    }
}

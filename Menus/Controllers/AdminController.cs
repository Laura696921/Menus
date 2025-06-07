using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menus.Data;
using Menus.Models;

namespace Menus.Controllers
{
    [Route("Admin")]  // Base route for this controller
    public class AdminController : Controller
    {
        private readonly MenuContext _context;

        public AdminController(MenuContext context)
        {
            _context = context;
        }

        // GET: /Admin/
        public IActionResult Index(string category)
        {
            var categories = new List<string> { "Салати", "Топли предястия", "Студени предястия", "Десерти", "Основни" };

            // Get all dishes or filter by category if provided
            var dishes = string.IsNullOrEmpty(category)
                ? _context.Dishes.ToList()
                : _context.Dishes.Where(d => d.Category == category).ToList();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = category;

            return View(dishes);
        }


        // GET: /Admin/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dish dish)
        {
            Console.WriteLine("POST Create called");

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Validation error on '{key}': {error.ErrorMessage}");
                    }
                }
                return View(dish);
            }

            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
            Console.WriteLine("Dish added");
            return RedirectToAction(nameof(Index));
        }




        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Dish dish)
        {
            int id = dish.Id;

            if (ModelState.IsValid)
            {
                var existingDish = await _context.Dishes.FindAsync(id);
                if (existingDish == null) return NotFound();

                existingDish.Name = dish.Name;
                existingDish.Description = dish.Description;
                existingDish.Price = dish.Price;
                existingDish.Category = dish.Category;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Admin");
            }
            return View(dish);
        }


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();

            return View(dish);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dish dish)
        {
            if (id != dish.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDish = await _context.Dishes.FindAsync(id);
                    if (existingDish == null) return NotFound();

                    // Update only the properties to avoid tracking issues
                    existingDish.Name = dish.Name;
                    existingDish.Description = dish.Description;
                    existingDish.Price = dish.Price;
                    existingDish.Category = dish.Category;

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Admin");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Dishes.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(dish);
        }




        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dish = await _context.Dishes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null) return NotFound();

            return View(dish);
        }

        // POST: /Admin/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}

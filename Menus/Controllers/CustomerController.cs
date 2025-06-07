using Menus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menus.Models;
using QRCoder;

namespace Menus.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MenuContext _context;

        public CustomerController(MenuContext context)
        {
            _context = context;
        }

        // GET: /Customer/Index
        public async Task<IActionResult> Index()
        {
            var dishes = await _context.Dishes
                .OrderBy(d => d.Category)
                .ThenBy(d => d.Name)
                .ToListAsync();

            var grouped = dishes
                .GroupBy(d => d.Category)
                .ToDictionary(g => g.Key, g => g.ToList());

            // ВЗЕМИ ПОСЛЕДНИТЕ 3 ОТЗИВА
            var latestReviews = await _context.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .Take(3)
                .ToListAsync();

            ViewData["Reviews"] = latestReviews;

            return View(grouped);
        }

        // GET: /Customer/Details/5
        public IActionResult Details(int id)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }


        // GET: /Customer/Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Dishes
                              .Select(d => d.Category)
                              .Distinct()
                              .ToListAsync();

            return View(categories);
        }

        // GET: /Customer/Category?name=Salads
        public async Task<IActionResult> Category(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            var dishes = await _context.Dishes
                               .Where(d => d.Category == name)
                               .ToListAsync();

            ViewData["Category"] = name;

            return View(dishes);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Here you can handle the form data,
            // e.g., save to DB, send email, log, etc.
            // For now, let's just display a success message.

            TempData["SuccessMessage"] = "Благодарим Ви за вашето съобщение! Ще се свържем с Вас скоро.";
            return RedirectToAction(nameof(Contact));
        }

        [HttpGet]
        public IActionResult LeaveReview()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LeaveReview(Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Reviews.Add(review);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Благодарим за вашия отзив!";
                return RedirectToAction("Index");
            }

            return View(review);
        }

        [HttpGet]
        public IActionResult Reservations()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Reservations(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                // Ensure the DateTime is in UTC
                reservation.ReservationDate = DateTime.SpecifyKind(reservation.ReservationDate, DateTimeKind.Utc);

                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Резервацията е успешна!";
                return RedirectToAction("Reservations");
            }

            return View(reservation);
        }

        public IActionResult GenerateQr()
        {
            string url = "https://localhost:7064/"; // change to your actual site

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);

            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(20);

            return File(qrCodeBytes, "image/png");
        }
    }
}
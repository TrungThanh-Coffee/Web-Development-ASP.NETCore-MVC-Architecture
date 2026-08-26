using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_cuoiky.Data;
using project_cuoiky.Models;
using System.Diagnostics;

namespace project_cuoiky.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly project_cuoikyContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            project_cuoikyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .ToList();

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                            ?? HttpContext.TraceIdentifier
            });
        }
    }
}
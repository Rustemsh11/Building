using Building.DAL;
using Building.Domain;
using Building.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Building.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BuildingContext _buildingContext;

        public HomeController(ILogger<HomeController> logger, BuildingContext buildingContext)
        {
            _buildingContext = buildingContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var res=_buildingContext.Employees.ToList();
            return View(res);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
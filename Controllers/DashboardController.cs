using Microsoft.AspNetCore.Mvc;
using Sagetaway.Data;
using Sagetaway.Models;

namespace sagetaway.Controllers
{

    public class DashboardController : Controller
    {
        private readonly SagetawayContext _context;
        public IActionResult Attractions()
        {
            ViewData["Layout"] = "~/Views/Shared/_AdminLayout.cshtml";
            return View();
        }
      
        public IActionResult Transport()
        {
            ViewData["Layout"] = "~/Views/Shared/_AdminLayout.cshtml";
            return View();
        }
       

        public DashboardController(SagetawayContext context)
        {
            _context = context;
        }

        public IActionResult Hotels()
        {
            ViewData["Layout"] = "~/Views/Shared/_AdminLayout.cshtml";
           
            return View();
        }

       
    }
}


using CSVFile.Data;
using Microsoft.AspNetCore.Mvc;

namespace CSVFile.Controllers
{
    public class CSVDataController : Controller
    {
        private readonly AppDbContext _context;

        public CSVDataController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Upload()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Delete() 
        {
            return View();
        }
    }
}

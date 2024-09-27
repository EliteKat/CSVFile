using CSVFile.Data;
using CSVFile.Models;
using CSVFile.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSVFile.Controllers
{
    public class CSVDataController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICSVDataService _iCSVDataService;

        public CSVDataController(AppDbContext context, ICSVDataService iCSVDataService)
        {
            _context = context;
            _iCSVDataService = iCSVDataService;
        }

        public async Task<IActionResult> Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError("csvFile", "Please upload a valid CSV file.");
                return View();
            }

            bool csvFileUploadResult = await _iCSVDataService.UploadAsync(csvFile);
            if(csvFileUploadResult)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid data.");

            var csvData = await _context.CSVDatas.FindAsync(id);
            if (csvData == null)
                return NotFound();

            return View(csvData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, CSVData updatedData)
        {
            if (id == Guid.Empty || updatedData == null)
                return BadRequest("Invalid data.");

            bool editResult = await _iCSVDataService.EditAsync(id, updatedData);
            if (editResult)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Unable to delete");
            return View(updatedData);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid data.");

            bool deleteResult = await _iCSVDataService.DeleteAsync(id);
            if (!deleteResult)
                ModelState.AddModelError("","Unable to delete");

            return RedirectToAction("Index", "Home"); 
        }
    }
}

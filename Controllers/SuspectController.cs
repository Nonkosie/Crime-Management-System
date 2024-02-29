using CrimeManagement.Data;
using CrimeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagement.Controllers
{
    [Authorize(Roles ="Police Officer, Captain")]
    public class SuspectController : Controller
    {
        private readonly ApplicationDbContext _dataContext;

        public SuspectController(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task<IActionResult> Upsert(int? suspectNo)
        {
            if (suspectNo == null || suspectNo == 0)
            {
                return View(new Suspect());
            }
            else
            {
                Suspect? suspect = await _dataContext.Suspects.FindAsync(suspectNo);
                return View(suspect);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Suspect record)
        {
            
            if (ModelState.IsValid)
            {
                // Check if the SuspectID already exists in the database.
                bool isSuspectIdExists = await _dataContext.Suspects.AnyAsync(s => s.SuspectId == record.SuspectId);

                if(record.SuspectNo == 0)
                {
                    if (isSuspectIdExists)
                    {

                        ModelState.AddModelError(nameof(record.SuspectId), "Suspect ID already exists.");
                        return View(record);
                    }

                    await _dataContext.AddAsync(record);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "New Suspect created successfully";
                    return RedirectToAction("New", "CriminalRecord", new { suspectNo = record.SuspectNo });
                }
                else
                {
                    _dataContext.Update(record);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Suspect updated successfully";
                    //return RedirectToAction("Index");
                    return RedirectToAction(nameof(HomeController.Index), "Home", new { suspectId = record.SuspectId });

                }
            }

            else if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var error = ModelState[key].Errors.FirstOrDefault();
                    if (error != null)
                    {
                        var errorMessage = error.ErrorMessage;
                        Console.WriteLine($"Validation Error for {key}: {errorMessage}");
                    }
                }

            }

            return View(record);
        }

    }
}

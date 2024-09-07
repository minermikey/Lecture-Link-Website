using Microsoft.AspNetCore.Mvc;
using PROG.Data;
using PROG.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PROG.Controllers
{
    public class InformationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public InformationController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Information/Create
        [Authorize(Roles = "LECTURER,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Information/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LecturerID,Firstname,Lastname,HouresWorked,Claimsperiodstart,Claimsperiodend,RatePerHour,DiscriptionOfWork")] Information information, IFormFile fileUpload)
        {
            if (ModelState.IsValid)
            {
                // Calculate TotalAmount
                information.TotalAmount = information.HouresWorked.HasValue ? information.HouresWorked.Value * information.RatePerHour : 0;

                // Handle file upload
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    // Define the path to save the file
                    var filePath = Path.Combine(_env.WebRootPath, "uploads", fileUpload.FileName);

                    // Ensure the 'uploads' directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    // Save the file to the specified path
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileUpload.CopyToAsync(fileStream);
                    }

                    // Save the file details in the model
                    information.FileName = fileUpload.FileName;
                }

                _context.Add(information);
                await _context.SaveChangesAsync();

                TempData["message"] = "Creation Successful";

                return RedirectToAction(nameof(Create));
            }
            ModelState.Clear();

            return View();
        }

        // GET: Information/Index
        [Authorize(Roles = "LECTURER,Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Information.ToListAsync());
        }

        // POST: Confirm Payment
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var information = await _context.Information.FindAsync(id);
            if (information != null)
            {
                information.PaymentStatus = "Confirmed";
                _context.Update(information);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Deny Payment
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DenyPayment(int id)
        {
            var information = await _context.Information.FindAsync(id);
            if (information != null)
            {
                information.PaymentStatus = "Denied";
                _context.Update(information);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "LECTURER,Admin")]
        public IActionResult SearchByLecturerID()
        {
            return View();
        }

       //  [Authorize(Roles = "Admin")]
        [HttpPost]
        [Authorize(Roles = "LECTURER,Admin")]
        public async Task<IActionResult> SearchByLecturerID(string lecturerID)
        {
            if (string.IsNullOrEmpty(lecturerID))
            {
                ModelState.AddModelError("", "Please enter a valid Lecturer ID.");
                return View();
            }

            // Convert lecturerID to int?
            int? lecturerIDInt = null;
            if (int.TryParse(lecturerID, out int parsedID))
            {
                lecturerIDInt = parsedID;
            }
            else
            {
                ModelState.AddModelError("", "Please enter a valid numeric Lecturer ID.");
                return View();
            }

            var informationList = await _context.Information
                .Where(info => info.LecturerID == lecturerIDInt)
                .ToListAsync();

            if (!informationList.Any())
            {
                ModelState.AddModelError("", "No records found for the specified Lecturer ID.");
            }

            return View("Index", informationList);
        }


         
        // GET: Information/Index
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            return View(await _context.Information.ToListAsync());
        }
         

    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialWelfarre.Data;
using SocialWelfarre.Data.Migrations;
using SocialWelfarre.Models;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace SocialWelfarre.Controllers
{ public class ApplicationFoodPacks1Controller : Controller
    { private readonly ApplicationDbContext _context; private readonly IWebHostEnvironment _hostingEnvironment; 
         public ApplicationFoodPacks1Controller(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: ApplicationFoodPacks1
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationFoodPack.ToListAsync());
        }

        // GET: ApplicationFoodPacks1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationFoodPack = await _context.ApplicationFoodPack
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationFoodPack == null)
            {
                return NotFound();
            }

            return View(applicationFoodPack);
        }

        // GET: ApplicationFoodPacks1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationFoodPacks1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Packs,Age,Barangay,Address,ContactNumber,Reason,Status")] ApplicationFoodPack applicationFoodPack, IFormFile brgyCertFile, IFormFile validIdFile)
        {
            // Validate uploaded files
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            var maxFileSize = 5 * 1024 * 1024; // 5 MB

            // Validate Barangay Certificate file
            if (brgyCertFile != null && brgyCertFile.Length > 0)
            {
                var extension = Path.GetExtension(brgyCertFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("brgyCertFile", "Only .jpg, .jpeg, .png, and .pdf files are allowed for Barangay Certificate.");
                    return View(applicationFoodPack);
                }
                if (brgyCertFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("brgyCertFile", "Barangay Certificate file size cannot exceed 5 MB.");
                    return View(applicationFoodPack);
                }

                var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "brgy_certs");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await brgyCertFile.CopyToAsync(stream);
                }

                applicationFoodPack.Brgy_Cert = fileName;
                applicationFoodPack.Brgy_Cert_Path = "/uploads/brgy_certs/" + fileName;
            }
            else
            {
                ModelState.AddModelError("brgyCertFile", "Please upload a Barangay Certificate.");
                return View(applicationFoodPack);
            }

            // Validate Valid ID file
            if (validIdFile != null && validIdFile.Length > 0)
            {
                var extension = Path.GetExtension(validIdFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("validIdFile", "Only .jpg, .jpeg, .png, and .pdf files are allowed for Valid ID.");
                    return View(applicationFoodPack);
                }
                if (validIdFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("validIdFile", "Valid ID file size cannot exceed 5 MB.");
                    return View(applicationFoodPack);
                }

                var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "valid_ids");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await validIdFile.CopyToAsync(stream);
                }

                applicationFoodPack.Valid_ID = fileName;
                applicationFoodPack.Valid_ID_Path = "/uploads/valid_ids/" + fileName;
            }
            else
            {
                ModelState.AddModelError("validIdFile", "Please upload a Valid ID.");
                return View(applicationFoodPack);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Add(applicationFoodPack);
            await _context.SaveChangesAsync();

            var activity = new AuditTrail
            {
                Action = "Create",
                TimeStamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = userId,
                Moduie = "Food Packs",
                AffectedTable = "Food Packs"
            };
            _context.Add(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ApplicationFoodPacks1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationFoodPack = await _context.ApplicationFoodPack.FindAsync(id);
            if (applicationFoodPack == null)
            {
                return NotFound();
            }
            return View(applicationFoodPack);
        }

        // POST: ApplicationFoodPacks1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Packs,Age,Barangay,Address,ContactNumber,Brgy_Cert,Brgy_Cert_Path,Valid_ID,Valid_ID_Path,Reason,Status")] ApplicationFoodPack applicationFoodPack, IFormFile brgyCertFile, IFormFile validIdFile)
        {
            if (id != applicationFoodPack.Id)
            {
                return NotFound();
            }

            try
            {
                var existingRecord = await _context.ApplicationFoodPack.FindAsync(id);
                if (existingRecord == null)
                {
                    return NotFound();
                }

                // Update fields
                existingRecord.FirstName = applicationFoodPack.FirstName;
                existingRecord.MiddleName = applicationFoodPack.MiddleName;
                existingRecord.LastName = applicationFoodPack.LastName;
                existingRecord.Packs = applicationFoodPack.Packs;
                existingRecord.Age = applicationFoodPack.Age;
                existingRecord.Barangay = applicationFoodPack.Barangay;
                existingRecord.Address = applicationFoodPack.Address;
                existingRecord.ContactNumber = applicationFoodPack.ContactNumber;
                existingRecord.Reason = applicationFoodPack.Reason;
                existingRecord.Status = applicationFoodPack.Status;

                // Validate and update Barangay Certificate
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
                var maxFileSize = 5 * 1024 * 1024; // 5 MB

                if (brgyCertFile != null && brgyCertFile.Length > 0)
                {
                    var extension = Path.GetExtension(brgyCertFile.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("brgyCertFile", "Only .jpg, .jpeg, .png, and .pdf files are allowed for Barangay Certificate.");
                        return View(applicationFoodPack);
                    }
                    if (brgyCertFile.Length > maxFileSize)
                    {
                        ModelState.AddModelError("brgyCertFile", "Barangay Certificate file size cannot exceed 5 MB.");
                        return View(applicationFoodPack);
                    }

                    // Delete old file if it exists
                    if (!string.IsNullOrEmpty(existingRecord.Brgy_Cert_Path))
                    {
                        var oldFilePath = Path.Combine(_hostingEnvironment.WebRootPath, existingRecord.Brgy_Cert_Path.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "brgy_certs");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(directoryPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await brgyCertFile.CopyToAsync(stream);
                    }

                    existingRecord.Brgy_Cert = fileName;
                    existingRecord.Brgy_Cert_Path = "/uploads/brgy_certs/" + fileName;
                }

                // Validate and update Valid ID
                if (validIdFile != null && validIdFile.Length > 0)
                {
                    var extension = Path.GetExtension(validIdFile.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("validIdFile", "Only .jpg, .jpeg, .png, and .pdf files are allowed for Valid ID.");
                        return View(applicationFoodPack);
                    }
                    if (validIdFile.Length > maxFileSize)
                    {
                        ModelState.AddModelError("validIdFile", "Valid ID file size cannot exceed 5 MB.");
                        return View(applicationFoodPack);
                    }

                    // Delete old file if it exists
                    if (!string.IsNullOrEmpty(existingRecord.Valid_ID_Path))
                    {
                        var oldFilePath = Path.Combine(_hostingEnvironment.WebRootPath, existingRecord.Valid_ID_Path.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "valid_ids");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(directoryPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await validIdFile.CopyToAsync(stream);
                    }

                    existingRecord.Valid_ID = fileName;
                    existingRecord.Valid_ID_Path = "/uploads/valid_ids/" + fileName;
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Update(existingRecord);
                await _context.SaveChangesAsync();

                var activity = new AuditTrail
                {
                    Action = "Edit",
                    TimeStamp = DateTime.Now,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    UserId = userId,
                    Moduie = "Food Packs",
                    AffectedTable = "Food Packs"
                };
                _context.Add(activity);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationFoodPackExists(applicationFoodPack.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: ApplicationFoodPacks1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationFoodPack = await _context.ApplicationFoodPack
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationFoodPack == null)
            {
                return NotFound();
            }

            return View(applicationFoodPack);
        }

        // POST: ApplicationFoodPacks1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationFoodPack = await _context.ApplicationFoodPack.FindAsync(id);
            if (applicationFoodPack != null)
            {
                // Delete associated files
                if (!string.IsNullOrEmpty(applicationFoodPack.Brgy_Cert_Path))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, applicationFoodPack.Brgy_Cert_Path.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                if (!string.IsNullOrEmpty(applicationFoodPack.Valid_ID_Path))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, applicationFoodPack.Valid_ID_Path.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.ApplicationFoodPack.Remove(applicationFoodPack);
            }

            var activity = new AuditTrail
            {
                Action = "Delete",
                TimeStamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = userId,
                Moduie = "Food Packs",
                AffectedTable = "Food Packs"
            };
            _context.Add(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationFoodPackExists(int id)
        {
            return _context.ApplicationFoodPack.Any(e => e.Id == id);
        }
    }
}
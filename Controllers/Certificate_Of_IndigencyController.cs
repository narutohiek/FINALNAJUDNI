using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialWelfarre.Data;
using SocialWelfarre.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocialWelfarre.Data.Migrations;
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
{
    public class Certificate_Of_IndigencyController : Controller
    {
        private readonly ApplicationDbContext _context; private readonly IWebHostEnvironment _hostingEnvironment;
        public Certificate_Of_IndigencyController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Certificate_Of_Indigency
        public async Task<IActionResult> Index()
        {
            return View(await _context.Certificate_Of_Indigencies.ToListAsync());
        }

        // GET: Certificate_Of_Indigency/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificate_Of_Indigency = await _context.Certificate_Of_Indigencies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certificate_Of_Indigency == null)
            {
                return NotFound();
            }

            return View(certificate_Of_Indigency);
        }

        // GET: Certificate_Of_Indigency/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Certificate_Of_Indigency/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Age,Barangay,No_Rquested,Address,ContactNumber,Reason,Status")] Certificate_Of_Indigency certificate_Of_Indigency, IFormFile brgyCertFile, IFormFile validIdFile)
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
                    return View(certificate_Of_Indigency);
                }
                if (brgyCertFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("brgyCertFile", "Barangay Certificate file size cannot exceed 5 MB.");
                    return View(certificate_Of_Indigency);
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

                certificate_Of_Indigency.Brgy_Cert = fileName;
                certificate_Of_Indigency.Brgy_Cert_Path = "/uploads/brgy_certs/" + fileName;
            }
            else
            {
                ModelState.AddModelError("brgyCertFile", "Please upload a Barangay Certificate.");
                return View(certificate_Of_Indigency);
            }

            // Validate Valid ID file
            if (validIdFile != null && validIdFile.Length > 0)
            {
                var extension = Path.GetExtension(validIdFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("validIdFile", "Only .jpg, .jpeg, .png, and .pdf files are allowed for Valid ID.");
                    return View(certificate_Of_Indigency);
                }
                if (validIdFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("validIdFile", "Valid ID file size cannot exceed 5 MB.");
                    return View(certificate_Of_Indigency);
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

                certificate_Of_Indigency.Valid_ID = fileName;
                certificate_Of_Indigency.Valid_ID_Path = "/uploads/valid_ids/" + fileName;
            }
            else
            {
                ModelState.AddModelError("validIdFile", "Please upload a Valid ID.");
                return View(certificate_Of_Indigency);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Add(certificate_Of_Indigency);
            await _context.SaveChangesAsync();

            var activity = new AuditTrail
            {
                Action = "Create",
                TimeStamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = userId,
                Moduie = "Certificate Of Indigency",
                AffectedTable = "Certificate Of Indigency"
            };
            _context.Add(activity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Certificate_Of_Indigency/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificate_Of_Indigency = await _context.Certificate_Of_Indigencies.FindAsync(id);
            if (certificate_Of_Indigency == null)
            {
                return NotFound();
            }
            return View(certificate_Of_Indigency);
        }

        // POST: Certificate_Of_Indigency/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Age,Barangay, No_Rquested, Address,ContactNumber,Brgy_Cert,Brgy_Cert_Path,Valid_ID,Valid_ID_Path,Reason,Status")] Certificate_Of_Indigency certificate_Of_Indigency, IFormFile brgyCertFile, IFormFile validIdFile)
        {
            if (id != certificate_Of_Indigency.Id)
            {
                return NotFound();
            }

            try
            {
                var existingRecord = await _context.Certificate_Of_Indigencies.FindAsync(id);
                if (existingRecord == null)
                {
                    return NotFound();
                }

                // Update fields
                existingRecord.FirstName = certificate_Of_Indigency.FirstName;
                existingRecord.MiddleName = certificate_Of_Indigency.MiddleName;
                existingRecord.LastName = certificate_Of_Indigency.LastName;
                existingRecord.Age = certificate_Of_Indigency.Age;
                existingRecord.Barangay = certificate_Of_Indigency.Barangay;
                existingRecord.No_Rquested = certificate_Of_Indigency.No_Rquested;
                existingRecord.Address = certificate_Of_Indigency.Address;
                existingRecord.ContactNumber = certificate_Of_Indigency.ContactNumber;
                existingRecord.Reason = certificate_Of_Indigency.Reason;
                existingRecord.Status = certificate_Of_Indigency.Status;

                // Validate and update Barangay Certificate
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
                var maxFileSize = 5 * 1024 * 1024; // 5 MB

                if (brgyCertFile != null && brgyCertFile.Length > 0)
                {
                    var extension = Path.GetExtension(brgyCertFile.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("brgyCertFile", "Only .jpg, .jpeg, .png, and .pdf files are allowed for Barangay Certificate.");
                        return View(certificate_Of_Indigency);
                    }
                    if (brgyCertFile.Length > maxFileSize)
                    {
                        ModelState.AddModelError("brgyCertFile", "Barangay Certificate file size cannot exceed 5 MB.");
                        return View(certificate_Of_Indigency);
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
                        return View(certificate_Of_Indigency);
                    }
                    if (validIdFile.Length > maxFileSize)
                    {
                        ModelState.AddModelError("validIdFile", "Valid ID file size cannot exceed 5 MB.");
                        return View(certificate_Of_Indigency);
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
                    Moduie = "Certificate Of Indigency",
                    AffectedTable = "Certificate Of Indigency"
                };
                _context.Add(activity);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Certificate_Of_IndigencyExists(certificate_Of_Indigency.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: Certificate_Of_Indigency/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var certificate_Of_Indigency = await _context.Certificate_Of_Indigencies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certificate_Of_Indigency == null)
            {
                return NotFound();
            }

            return View(certificate_Of_Indigency);
        }

        // POST: Certificate_Of_Indigency/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var certificate_Of_Indigency = await _context.Certificate_Of_Indigencies.FindAsync(id);
            if (certificate_Of_Indigency != null)
            {
                // Delete associated files
                if (!string.IsNullOrEmpty(certificate_Of_Indigency.Brgy_Cert_Path))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, certificate_Of_Indigency.Brgy_Cert_Path.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                if (!string.IsNullOrEmpty(certificate_Of_Indigency.Valid_ID_Path))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, certificate_Of_Indigency.Valid_ID_Path.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Certificate_Of_Indigencies.Remove(certificate_Of_Indigency);
            }

            var activity = new AuditTrail
            {
                Action = "Delete",
                TimeStamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = userId,
                Moduie = "Certificate Of Indigency",
                AffectedTable = "Certificate Of Indigency"
            };
            _context.Add(activity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool Certificate_Of_IndigencyExists(int id)
        {
            return _context.Certificate_Of_Indigencies.Any(e => e.Id == id);
        }
    }
}
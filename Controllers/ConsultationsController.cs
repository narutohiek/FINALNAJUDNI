using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialWelfarre.Data;
using SocialWelfarre.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SocialWelfarre.Controllers
{
    public class ConsultationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ConsultationsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Consultations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Consultations.ToListAsync());
        }

        // GET: Consultations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // GET: Consultations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consultations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,First_Name,Middle_Name,Last_Name,Consultation_Date,Consultation_Time,Consultation_status")] Consultation consultation,
            IFormFile brgyCertFile,
            IFormFile proofSoloParentFile,
            IFormFile birthCertFile,
            IFormFile validIdFile,
            IFormFile x1PicFile)
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
                    return View(consultation);
                }
                if (brgyCertFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("brgyCertFile", "Barangay Certificate file size cannot exceed 5 MB.");
                    return View(consultation);
                }

                var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", "brgy_certs");
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

                consultation.Brgy_Cert = fileName;
                consultation.Brgy_Cert_Path = "/Uploads/brgy_certs/" + fileName;
            }
            else
            {
                ModelState.AddModelError("brgyCertFile", "Please upload a Barangay Certificate.");
                return View(consultation);
            }

            // Validate Proof of Solo Parent file
            if (proofSoloParentFile != null && proofSoloParentFile.Length > 0)
            {
                var extension = Path.GetExtension(proofSoloParentFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("proofSoloParentFile", "Only .jpg, .jpeg, .png, and .pdf files are allowed for Proof of Solo Parent.");
                    return View(consultation);
                }
                if (proofSoloParentFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("proofSoloParentFile", "Proof of Solo Parent file size cannot exceed 5 MB.");
                    return View(consultation);
                }

                var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", "solo_parent_proofs");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await proofSoloParentFile.CopyToAsync(stream);
                }

                consultation.Proof_SoloParent = fileName;
                consultation.Proof_SoloParent_Path = "/Uploads/solo_parent_proofs/" + fileName;
            }
            else
            {
                ModelState.AddModelError("proofSoloParentFile", "Please upload a Proof of Solo Parent.");
                return View(consultation);
            }

            // Validate Birth Certificate file
            if (birthCertFile != null && birthCertFile.Length > 0)
            {
                var extension = Path.GetExtension(birthCertFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("birthCertFile", "Only .jpg, .jpeg, .png, and .pdf files are allowed for Birth Certificate.");
                    return View(consultation);
                }
                if (birthCertFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("birthCertFile", "Birth Certificate file size cannot exceed 5 MB.");
                    return View(consultation);
                }

                var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", "birth_certs");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await birthCertFile.CopyToAsync(stream);
                }

                consultation.Birth_Cert = fileName;
                consultation.Birth_Cert_Path = "/Uploads/birth_certs/" + fileName;
            }
            else
            {
                ModelState.AddModelError("birthCertFile", "Please upload a Birth Certificate.");
                return View(consultation);
            }

            // Validate Valid ID file
            if (validIdFile != null && validIdFile.Length > 0)
            {
                var extension = Path.GetExtension(validIdFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("validIdFile", "Only .jpg, .jpeg, .png, and .pdf files are allowed for Valid ID.");
                    return View(consultation);
                }
                if (validIdFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("validIdFile", "Valid ID file size cannot exceed 5 MB.");
                    return View(consultation);
                }

                var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", "valid_ids");
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

                consultation.Valid_ID = fileName;
                consultation.Valid_ID_Path = "/Uploads/valid_ids/" + fileName;
            }
            else
            {
                ModelState.AddModelError("validIdFile", "Please upload a Valid ID.");
                return View(consultation);
            }

            // Validate 1x1 Picture file
            if (x1PicFile != null && x1PicFile.Length > 0)
            {
                var extension = Path.GetExtension(x1PicFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("x1PicFile", "Only .jpg, .jpeg, .png, and .pdf files are allowed for 1x1 Picture.");
                    return View(consultation);
                }
                if (x1PicFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("x1PicFile", "1x1 Picture file size cannot exceed 5 MB.");
                    return View(consultation);
                }

                var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", "x1_pics");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await x1PicFile.CopyToAsync(stream);
                }

                consultation.x1_Pic = fileName;
                consultation.x1_Pic_Path = "/Uploads/x1_pics/" + fileName;
            }
            else
            {
                ModelState.AddModelError("x1PicFile", "Please upload a 1x1 Picture.");
                return View(consultation);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Add(consultation);
            await _context.SaveChangesAsync();

            var activity = new AuditTrail
            {
                Action = "Create",
                TimeStamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = userId,
                Moduie = "Consultation",
                AffectedTable = "Consultations"
            };
            _context.Add(activity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Consultations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }
            return View(consultation);
        }

        // POST: Consultations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,First_Name,Middle_Name,Last_Name,Brgy_Cert,Brgy_Cert_Path,Proof_SoloParent,Proof_SoloParent_Path,Birth_Cert,Birth_Cert_Path,Valid_ID,Valid_ID_Path,x1_Pic,x1_Pic_Path,Consultation_Date,Consultation_Time,Consultation_status")] Consultation consultation)
        {
            if (id != consultation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultationExists(consultation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(consultation);
        }

        // GET: Consultations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // POST: Consultations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation != null)
            {
                _context.Consultations.Remove(consultation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultationExists(int id)
        {
            return _context.Consultations.Any(e => e.Id == id);
        }
    }
}
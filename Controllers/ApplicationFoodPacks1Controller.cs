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

namespace SocialWelfarre.Controllers
{
    public class ApplicationFoodPacks1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationFoodPacks1Controller(ApplicationDbContext context)
        {
            _context = context;
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Packs,Age,Barangay,Address,ContactNumber,Brgy_Cert,Valid_ID,Reason,Status")] ApplicationFoodPack applicationFoodPack)
        {
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Packs,Age,Barangay,Address,ContactNumber,Brgy_Cert,Valid_ID,Reason,Status")] ApplicationFoodPack applicationFoodPack)
        {
            if (id != applicationFoodPack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // <-- get user ID

                    _context.Update(applicationFoodPack);
                    await _context.SaveChangesAsync(); // <-- save update first

                    // INSERT your AuditTrail HERE after successful update
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
                    await _context.SaveChangesAsync(); // <-- save audit trail
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

                return RedirectToAction(nameof(Index));
            }
            return View(applicationFoodPack);
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // <-- get user ID
            var applicationFoodPack = await _context.ApplicationFoodPack.FindAsync(id);
            if (applicationFoodPack != null)
            {
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

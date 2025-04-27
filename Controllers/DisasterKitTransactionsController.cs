using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialWelfarre.Data;
using SocialWelfarre.Models;

namespace SocialWelfarre.Controllers
{
    public class DisasterKitTransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisasterKitTransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DisasterKitTransactions
        public async Task<IActionResult> Index()
        {
            return View(await _context.DisasterKitTransactions.ToListAsync());
        }

        // GET: DisasterKitTransactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disasterKitTransaction = await _context.DisasterKitTransactions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disasterKitTransaction == null)
            {
                return NotFound();
            }

            return View(disasterKitTransaction);
        }

        // GET: DisasterKitTransactions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DisasterKitTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Barangay,Reason,TransactionDate,TransactionTime,NumberOfPacks")] DisasterKitTransaction disasterKitTransaction)
        {
         
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(disasterKitTransaction);
                await _context.SaveChangesAsync();
            
            
            var activity = new AuditTrail
            {

                Action = "Create",
                TimeStamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = userId,
                Moduie = "Disaster Kit",
                AffectedTable = "Disaster Kit"
            };
            _context.Add(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: DisasterKitTransactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disasterKitTransaction = await _context.DisasterKitTransactions.FindAsync(id);
            if (disasterKitTransaction == null)
            {
                return NotFound();
            }
            return View(disasterKitTransaction);
        }

        // POST: DisasterKitTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Barangay,Reason,TransactionDate,TransactionTime,NumberOfPacks")] DisasterKitTransaction disasterKitTransaction)
        {
            if (id != disasterKitTransaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    _context.Update(disasterKitTransaction);
                    await _context.SaveChangesAsync(); // Save the update first

                    // Insert the AuditTrail after successful update
                    var activity = new AuditTrail
                    {
                        Action = "Edit",
                        TimeStamp = DateTime.Now,
                        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        UserId = userId,
                        Moduie = "Disaster Kit Transaction",
                        AffectedTable = "DisasterKitTransaction"
                    };
                    _context.Add(activity);
                    await _context.SaveChangesAsync(); // Save the audit trail
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisasterKitTransactionExists(disasterKitTransaction.Id))
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
            return View(disasterKitTransaction);
        }

        // GET: DisasterKitTransactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disasterKitTransaction = await _context.DisasterKitTransactions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disasterKitTransaction == null)
            {
                return NotFound();
            }

            return View(disasterKitTransaction);
        }

        // POST: DisasterKitTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var disasterKitTransaction = await _context.DisasterKitTransactions.FindAsync(id);
            if (disasterKitTransaction != null)
            {
                _context.DisasterKitTransactions.Remove(disasterKitTransaction);
            }
            var activity = new AuditTrail
            {
                Action = "Delete",
                TimeStamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = userId,
                Moduie = "Disaster Kit Transaction",
                AffectedTable = "DisasterKitTransaction"
            };
            _context.Add(activity);
            await _context.SaveChangesAsync(); // Save the audit trail
            return RedirectToAction(nameof(Index));
        }

        private bool DisasterKitTransactionExists(int id)
        {
            return _context.DisasterKitTransactions.Any(e => e.Id == id);
        }
    }
}

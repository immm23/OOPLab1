using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using OOPLab1.Models;

namespace OOPLab1.Controllers
{
    public class PharmasiesController : Controller
    {
        private readonly PillsContext _context;

        public PharmasiesController(PillsContext context)
        {
            _context = context;
        }

        // GET: Pharmasies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pharmasies.Include(p => p.Pills).ToListAsync());
        }

        // GET: Pharmasies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pharmasies == null)
            {
                return NotFound();
            }

            var pharmasy = await _context.Pharmasies
                .Include(p => p.Pills)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pharmasy == null)
            {
                return NotFound();
            }

            return View(pharmasy);
        }

        // GET: Pharmasies/Create
        public IActionResult Create()
        {
            ViewData["Pills"] = new MultiSelectList(_context.Pills, "Id", "Name");
            return View();
        }

        // POST: Pharmasies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Adress, SelectedPills,PhoneNumber,OwnerName")] Pharmasy pharmasy)
        {
            if (ModelState.IsValid)
            {
                List<Pill> pills = new List<Pill>();

                if(pharmasy.SelectedPills!= null)
                {
                    foreach (var item in pharmasy.SelectedPills)
                    {
                        var dbItem = _context.Pills.First(p => p.Id == item);
                        pills.Add(dbItem);
                    }
                }

                pharmasy.Pills.AddRange(pills);
                _context.Add(pharmasy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Pills"] = new MultiSelectList(_context.Pills, "Id", "Name", pharmasy.SelectedPills);
            return View(pharmasy);
        }

        // GET: Pharmasies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pharmasies == null)
            {
                return NotFound();
            }

            var pharmasy = await _context.Pharmasies
                .Include(p => p.Pills)
                .FirstAsync(p => p.Id == id);
            if (pharmasy == null)
            {
                return NotFound();
            }

            List<int> pillsSelected = new List<int>();
            pharmasy.Pills.ToList().ForEach(p => pillsSelected.Add(p.Id));

            ViewData["Pills"] = new MultiSelectList(_context.Pills, "Id", "Name", pillsSelected);
            return View(pharmasy);
        }

        // POST: Pharmasies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Adress,SelectedPills, PhoneNumber,OwnerName")] Pharmasy pharmasy)
        {
            if (id != pharmasy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    List<Pill> pills = new List<Pill>();

                    if(pharmasy.SelectedPills != null)
                    {
                        foreach (var item in pharmasy.SelectedPills)
                        {
                            var dbItem = _context.Pills.First(p => p.Id == item);
                            pills.Add(dbItem);
                        }
                    }
                   

                    pharmasy.Pills.Clear();
                    pharmasy.Pills.AddRange(pills);

                    _context.Update(pharmasy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PharmasyExists(pharmasy.Id))
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
            return View(pharmasy);
        }

        // GET: Pharmasies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pharmasies == null)
            {
                return NotFound();
            }

            var pharmasy = await _context.Pharmasies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pharmasy == null)
            {
                return NotFound();
            }

            return View(pharmasy);
        }

        // POST: Pharmasies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pharmasies == null)
            {
                return Problem("Entity set 'PillsContext.Pharmasies'  is null.");
            }
            var pharmasy = await _context.Pharmasies.FindAsync(id);
            if (pharmasy != null)
            {
                _context.Pharmasies.Remove(pharmasy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PharmasyExists(int id)
        {
          return (_context.Pharmasies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

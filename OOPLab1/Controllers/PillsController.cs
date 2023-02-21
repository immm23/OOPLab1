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
    public class PillsController : Controller
    {
        private readonly PillsContext _context;

        public PillsController(PillsContext context)
        {
            _context = context;
        }

        // GET: Pills
        public async Task<IActionResult> Index()
        {
            var pillsContext = _context.Pills.Include(p => p.ClassNavigation)
                .Include(p => p.Illnes)
                .Include(P => P.Pharmasies);

            ViewData["Class"] = new SelectList(_context.Classes, "Id", "Name");
            ViewData["Pharmasies"] = new MultiSelectList(_context.Pharmasies, "Id", "Name");
            ViewData["Illnes"] = new MultiSelectList(_context.Ilnesses, "Id", "Name");

            return View(await pillsContext.ToListAsync());
        }

        // GET: Pills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pills == null)
            {
                return NotFound();
            }

            var pill = await _context.Pills
                .Include(p => p.ClassNavigation)
                .Include(p => p.Illnes)
                .Include(p => p.Pharmasies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pill == null)
            {
                return NotFound();
            }

            return View(pill);
        }

        // GET: Pills/Create
        public IActionResult Create()
        {
            ViewData["Class"] = new SelectList(_context.Classes, "Id", "Name");
            ViewData["Pharmasies"] = new MultiSelectList(_context.Pharmasies, "Id", "Name");
            ViewData["Illnes"] = new MultiSelectList(_context.Ilnesses, "Id", "Name");
            return View();
        }

        // POST: Pills/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Class,SelectedPharmasies,SelectedIllnes,SideEffects,ExpiryDate")] Pill pill)
        {
            pill.ClassNavigation = _context.Classes.First(p => p.Id == pill.Class);
            if (ModelState.IsValid)
            {
                List<Ilness> ilnesses = new List<Ilness>();
                foreach (var item in pill.SelectedIllnes)
                {
                    var dbItem = _context.Ilnesses.First(p => p.Id == item);
                    ilnesses.Add(dbItem);
                }

                List<Pharmasy> pharmasies = new List<Pharmasy>();
                foreach (var item in pill.SelectedPharmasies)
                {
                    var dbItem = _context.Pharmasies.First(p => p.Id == item);
                    pharmasies.Add(dbItem);
                }

                pill.Illnes.AddRange(ilnesses);
                pill.Pharmasies.AddRange(pharmasies);
                _context.Add(pill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Class"] = new SelectList(_context.Classes, "Id", "Name");
            ViewData["Pharmasies"] = new MultiSelectList(_context.Pharmasies, "Id", "Name", pill.SelectedPharmasies);
            ViewData["Illnes"] = new MultiSelectList(_context.Ilnesses, "Id", "Name", pill.SelectedIllnes);
            return View(pill);
        }

        // GET: Pills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pills == null)
            {
                return NotFound();
            }

            var pill = await _context.Pills.Include(p => p.Illnes)
                .Include(p => p.Pharmasies)
                .Include(p => p.ClassNavigation)
                .FirstAsync(p => p.Id == id);
            if (pill == null)
            {
                return NotFound();
            }

            List<int> selectedPharmasies = new List<int>();
            List<int> selectedIllnesses = new List<int>();

            pill.Illnes.ToList().ForEach(p => selectedIllnesses.Add(p.Id));
            pill.Pharmasies.ToList().ForEach(p => selectedPharmasies.Add(p.Id));


            ViewData["Class"] = new SelectList(_context.Classes, "Id", "Name");
            ViewData["Pharmasies"] = new MultiSelectList(_context.Pharmasies, "Id", "Name", selectedPharmasies);
            ViewData["Illnes"] = new MultiSelectList(_context.Ilnesses, "Id", "Name", selectedIllnesses);

            return View(pill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Class, SelectedPharmasies, SelectedIllnes, SideEffects, ExpiryDate")] Pill pill)
        {
            pill.ClassNavigation = _context.Classes.First(p => p.Id == pill.Class);
            if (id != pill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    List<Ilness> ilnesses = new List<Ilness>();
                    if(pill.SelectedIllnes != null)
                    {
                        foreach (var item in pill.SelectedIllnes)
                        {
                            var dbItem = _context.Ilnesses.First(p => p.Id == item);
                            ilnesses.Add(dbItem);
                        }
                    }

                    List<Pharmasy> pharmasies = new List<Pharmasy>();
                    if (pill.SelectedPharmasies != null)
                    {
                        foreach (var item in pill.SelectedPharmasies)
                        {

                            var dbItem = _context.Pharmasies.First(p => p.Id == item);
                            pharmasies.Add(dbItem);
                        }

                    }

                    pill.Illnes.Clear();
                    pill.Pharmasies.Clear();
                    pill.Illnes.AddRange(ilnesses);
                    pill.Pharmasies.AddRange(pharmasies);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PillExists(pill.Id))
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
            ViewData["Class"] = new SelectList(_context.Classes, "Id", "Name");
            ViewData["Pharmasies"] = new MultiSelectList(_context.Pharmasies, "Id", "Name", pill.SelectedPharmasies);
            ViewData["Illnes"] = new MultiSelectList(_context.Ilnesses, "Id", "Name", pill.SelectedIllnes);
            return View(pill);
        }

        // GET: Pills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pills == null)
            {
                return NotFound();
            }

            var pill = await _context.Pills
                .Include(p => p.ClassNavigation)
                .Include(p => p.Illnes)
                .Include(p => p.Pharmasies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pill == null)
            {
                return NotFound();
            }

            return View(pill);
        }

        // POST: Pills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pills == null)
            {
                return Problem("Entity set 'PillsContext.Pills'  is null.");
            }
            var pill = await _context.Pills.FindAsync(id);
            if (pill != null)
            {
                _context.Pills.Remove(pill);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PillExists(int id)
        {
          return (_context.Pills?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

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
    public class IlnessesController : Controller
    {
        private readonly PillsContext _context;

        public IlnessesController(PillsContext context)
        {
            _context = context;
        }

        // GET: Ilnesses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ilnesses.Include(p => p.Pills).ToListAsync());
        }

        // GET: Ilnesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ilnesses == null)
            {
                return NotFound();
            }

            var ilness = await _context.Ilnesses
                .Include(p => p.Pills)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ilness == null)
            {
                return NotFound();
            }

            return View(ilness);
        }

        // GET: Ilnesses/Create
        public IActionResult Create()
        {
            ViewData["Pills"] = new MultiSelectList(_context.Pills, "Id", "Name");
            return View();
        }

        // POST: Ilnesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Symptoms,Description, PillsSelected")] Ilness ilness)
        {
            if (ModelState.IsValid)
            {
                List<Pill> pills = new List<Pill>();
                if(ilness.PillsSelected != null)
                {
                    foreach (var item in ilness.PillsSelected)
                    {
                        var dbItem = _context.Pills.First(p => p.Id == item);
                        pills.Add(dbItem);
                    }
                }
               

                ilness.Pills.AddRange(pills);

                _context.Add(ilness);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Pills"] = new MultiSelectList(_context.Pills, "Id", "Name", ilness.PillsSelected);
            return View(ilness);
        }

        // GET: Ilnesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ilnesses == null)
            {
                return NotFound();
            }

            var ilness = await _context.Ilnesses
                .Include(p => p.Pills)
                .FirstAsync(p => p.Id == id);
            if (ilness == null)
            {
                return NotFound();
            }

            List<int> pillsSelected = new List<int>();
            ilness.Pills.ToList().ForEach(p => pillsSelected.Add(p.Id));

            ViewData["Pills"] = new MultiSelectList(_context.Pills, "Id", "Name", pillsSelected);
            return View(ilness);
        }

        // POST: Ilnesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PillsSelected,Symptoms,Description")] Ilness ilness)
        {
            if (id != ilness.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    List<Pill> pills = new List<Pill>();

                    if(ilness.PillsSelected != null)
                    {
                        foreach (var item in ilness.PillsSelected)
                        {
                            var dbItem = _context.Pills.First(p => p.Id == item);
                            pills.Add(dbItem);
                        }
                    }


                    ilness.Pills.Clear();
                    ilness.Pills.AddRange(pills);

                    _context.Update(ilness);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IlnessExists(ilness.Id))
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
            ViewData["Pills"] = new SelectList(_context.Pills, "Id", "Name");
            return View(ilness);
        }

        // GET: Ilnesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ilnesses == null)
            {
                return NotFound();
            }

            var ilness = await _context.Ilnesses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ilness == null)
            {
                return NotFound();
            }

            return View(ilness);
        }

        // POST: Ilnesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ilnesses == null)
            {
                return Problem("Entity set 'PillsContext.Ilnesses'  is null.");
            }
            var ilness = await _context.Ilnesses.FindAsync(id);
            if (ilness != null)
            {
                _context.Ilnesses.Remove(ilness);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IlnessExists(int id)
        {
          return (_context.Ilnesses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using beadando.Data;
using beadando.Models;

namespace toystore.Controllers
{
    public class DownloadController : Controller
    {
        private readonly StoreContext _context;

        public DownloadController(StoreContext context)
        {
            _context = context;
        }

        // GET: Download
        public async Task<IActionResult> Index()
        {
              return _context.Downloadables != null ? 
                          View(await _context.Downloadables.ToListAsync()) :
                          Problem("Entity set 'StoreContext.Downloadables'  is null.");
        }

        // GET: Download/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Downloadables == null)
            {
                return NotFound();
            }

            var downloadable = await _context.Downloadables
                .FirstOrDefaultAsync(m => m.ID == id);
            if (downloadable == null)
            {
                return NotFound();
            }

            return View(downloadable);
        }

        // GET: Download/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Download/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Path")] Downloadable downloadable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(downloadable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(downloadable);
        }

        // GET: Download/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Downloadables == null)
            {
                return NotFound();
            }

            var downloadable = await _context.Downloadables.FindAsync(id);
            if (downloadable == null)
            {
                return NotFound();
            }
            return View(downloadable);
        }

        // POST: Download/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Path")] Downloadable downloadable)
        {
            if (id != downloadable.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(downloadable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DownloadableExists(downloadable.ID))
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
            return View(downloadable);
        }

        // GET: Download/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Downloadables == null)
            {
                return NotFound();
            }

            var downloadable = await _context.Downloadables
                .FirstOrDefaultAsync(m => m.ID == id);
            if (downloadable == null)
            {
                return NotFound();
            }

            return View(downloadable);
        }

        // POST: Download/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Downloadables == null)
            {
                return Problem("Entity set 'StoreContext.Downloadables'  is null.");
            }
            var downloadable = await _context.Downloadables.FindAsync(id);
            if (downloadable != null)
            {
                _context.Downloadables.Remove(downloadable);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DownloadableExists(int id)
        {
          return (_context.Downloadables?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

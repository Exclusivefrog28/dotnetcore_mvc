using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using beadando.Data;
using beadando.Models;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;

namespace beadando.Controllers
{
    public class ToysController : Controller
    {
        private readonly StoreContext _context;

        public ToysController(StoreContext context)
        {
            _context = context;
        }

        // GET: Toys
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.Categories.Include(c => c.Toys);
            //return 3 lists, for 3 categories
            return View(await storeContext.ToListAsync());
        }

        // GET: Toys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Toys == null)
            {
                return NotFound();
            }

            var toy = await _context.Toys
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (toy == null)
            {
                return NotFound();
            }

            return View(toy);
        }

        // GET: Toys/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID");
            return View();
        }

        // POST: Toys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,Price,Image,CategoryID")] Toy toy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            Console.WriteLine(ModelState.ToJson());
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID", toy.CategoryID);
            return View(toy);
        }

        // GET: Toys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Toys == null)
            {
                return NotFound();
            }

            var toy = await _context.Toys.FindAsync(id);
            if (toy == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID", toy.CategoryID);
            return View(toy);
        }

        // POST: Toys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,Price,Image,CategoryID")] Toy toy)
        {
            if (id != toy.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToyExists(toy.ID))
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID", toy.CategoryID);
            return View(toy);
        }

        // GET: Toys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Toys == null)
            {
                return NotFound();
            }

            var toy = await _context.Toys
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (toy == null)
            {
                return NotFound();
            }

            return View(toy);
        }

        // POST: Toys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Toys == null)
            {
                return Problem("Entity set 'StoreContext.Toys'  is null.");
            }
            var toy = await _context.Toys.FindAsync(id);
            if (toy != null)
            {
                _context.Toys.Remove(toy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToyExists(int id)
        {
          return (_context.Toys?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

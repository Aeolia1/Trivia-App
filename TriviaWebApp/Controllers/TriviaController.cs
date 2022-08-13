using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TriviaWebApp.Data;
using TriviaWebApp.Models;

namespace TriviaWebApp.Controllers
{
    public class TriviaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TriviaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trivia
        public async Task<IActionResult> Index()
        {
              return _context.Trivia != null ? 
                          View(await _context.Trivia.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Trivia'  is null.");
        }

        // GET: Trivia/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
                        
        }

        // POST: Trivia/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index",await _context.Trivia.Where( t => t.TriviaQuestion.Contains(SearchPhrase)).ToListAsync());

        }

        // GET: Trivia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trivia == null)
            {
                return NotFound();
            }

            var trivia = await _context.Trivia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trivia == null)
            {
                return NotFound();
            }

            return View(trivia);
        }

        // GET: Trivia/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trivia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TriviaQuestion,TriviaAnswer")] Trivia trivia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trivia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trivia);
        }

        // GET: Trivia/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trivia == null)
            {
                return NotFound();
            }

            var trivia = await _context.Trivia.FindAsync(id);
            if (trivia == null)
            {
                return NotFound();
            }
            return View(trivia);
        }

        // POST: Trivia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TriviaQuestion,TriviaAnswer")] Trivia trivia)
        {
            if (id != trivia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trivia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TriviaExists(trivia.Id))
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
            return View(trivia);
        }

        // GET: Trivia/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trivia == null)
            {
                return NotFound();
            }

            var trivia = await _context.Trivia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trivia == null)
            {
                return NotFound();
            }

            return View(trivia);
        }

        // POST: Trivia/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Trivia == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Trivia'  is null.");
            }
            var trivia = await _context.Trivia.FindAsync(id);
            if (trivia != null)
            {
                _context.Trivia.Remove(trivia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TriviaExists(int id)
        {
          return (_context.Trivia?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Controllers
{
    public class HallMoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HallMoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HallMovies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HallMovies.Include(h => h.Hall).Include(h => h.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HallMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hallMovie = await _context.HallMovies
                .Include(h => h.Hall)
                .Include(h => h.Movie)
                .FirstOrDefaultAsync(m => m.HallId == id);
            if (hallMovie == null)
            {
                return NotFound();
            }

            return View(hallMovie);
        }

        // GET: HallMovies/Create
        public IActionResult Create()
        {
            ViewData["HallId"] = new SelectList(_context.Halls, "HallId", "HallName"); // afișează numele sălii
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title"); // afișează titlul filmului
            return View();
        }


        // POST: HallMovies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HallId,MovieId,StartDate,EndDate")] HallMovie hallMovie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hallMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HallId"] = new SelectList(_context.Halls, "HallId", "HallName", hallMovie.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", hallMovie.MovieId);

            return View(hallMovie);
        }

        // GET: HallMovies/Edit/5
        public async Task<IActionResult> Edit(int? hallId, int? movieId)
        {
            if (hallId == null || movieId == null)
            {
                return NotFound();
            }

            var hallMovie = await _context.HallMovies.FindAsync(hallId, movieId);
            if (hallMovie == null)
            {
                return NotFound();
            }

            ViewData["HallId"] = new SelectList(_context.Halls, "HallId", "HallName", hallMovie.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", hallMovie.MovieId);
            return View(hallMovie);
        }


        // POST: HallMovies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int hallId, int movieId, [Bind("HallId,MovieId,StartDate,EndDate")] HallMovie hallMovie)
        {
            if (hallId != hallMovie.HallId || movieId != hallMovie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hallMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.HallMovies.Any(e => e.HallId == hallId && e.MovieId == movieId))
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

            ViewData["HallId"] = new SelectList(_context.Halls, "HallId", "HallName", hallMovie.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", hallMovie.MovieId);
            return View(hallMovie);
        }


        // GET: HallMovies/Delete
        public async Task<IActionResult> Delete(int? hallId, int? movieId)
        {
            if (hallId == null || movieId == null)
            {
                return NotFound();
            }

            var hallMovie = await _context.HallMovies
                .Include(h => h.Hall)
                .Include(h => h.Movie)
                .FirstOrDefaultAsync(m => m.HallId == hallId && m.MovieId == movieId);

            if (hallMovie == null)
            {
                return NotFound();
            }

            return View(hallMovie);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int hallId, int movieId)
        {
            var hallMovie = await _context.HallMovies
                .FirstOrDefaultAsync(m => m.HallId == hallId && m.MovieId == movieId);

            if (hallMovie != null)
            {
                _context.HallMovies.Remove(hallMovie);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
    
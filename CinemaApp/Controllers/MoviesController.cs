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
    public class MoviesController : Controller
    {
        private readonly CinemaDbContext _context;

        public MoviesController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        public async Task<IActionResult> Create()
        {
            var actors = await _context.Actors.ToListAsync();
            ViewBag.Actors = actors;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();

                if (movie.SelectedActorIds != null)
                {
                    foreach (var actorId in movie.SelectedActorIds)
                    {
                        _context.MovieActors.Add(new MovieActor
                        {
                            MovieId = movie.MovieId,
                            ActorId = actorId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Actors = await _context.Actors.ToListAsync();
            return View(movie);
        }


        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
                return NotFound();

            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
                return NotFound();

            // Preluăm toți actorii pentru view
            var allActors = await _context.Actors.ToListAsync();
            ViewBag.Actors = allActors;

            // Selectăm actorii deja asociați cu filmul
            movie.SelectedActorIds = movie.MovieActors.Select(ma => ma.ActorId).ToList();

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.MovieId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizăm filmul
                    _context.Update(movie);
                    await _context.SaveChangesAsync();

                    // Ștergem actorii vechi
                    var existingActors = _context.MovieActors.Where(ma => ma.MovieId == movie.MovieId);
                    _context.MovieActors.RemoveRange(existingActors);
                    await _context.SaveChangesAsync();

                    // Adăugăm actorii noi selectați
                    if (movie.SelectedActorIds != null)
                    {
                        foreach (var actorId in movie.SelectedActorIds)
                        {
                            _context.MovieActors.Add(new MovieActor
                            {
                                MovieId = movie.MovieId,
                                ActorId = actorId
                            });
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Movies.Any(e => e.MovieId == movie.MovieId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Actors = await _context.Actors.ToListAsync();
            return View(movie);
        }


        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}

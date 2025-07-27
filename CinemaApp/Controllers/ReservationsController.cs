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
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservations.Include(r => r.Hall).Include(r => r.HallMovie).Include(r => r.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Hall)
                .Include(r => r.HallMovie)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            var hallMovieOptions = _context.HallMovies
                .Include(hm => hm.Hall)
                .Include(hm => hm.Movie)
                .Select(hm => new SelectListItem
                {
                    Value = $"{hm.HallId}|{hm.MovieId}",
                    Text = $"{hm.Movie.Title} - {hm.Hall.HallName} ({hm.StartDate:dd.MM.yyyy HH:mm})"
                }).ToList();

            ViewBag.HallMovieOptions = hallMovieOptions;

            return View();
        }


        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string hallMovieValue, [Bind("ReservationId,FirstName,LastName,PhoneNumber,SeatNumber,ReservationDate")] Reservation reservation)
        {
            if (string.IsNullOrEmpty(hallMovieValue) || !hallMovieValue.Contains("|"))
            {
                ModelState.AddModelError("HallMovie", "Te rugăm să selectezi o proiecție (Film + Sală).");
            }
            else
            {
                var parts = hallMovieValue.Split('|');
                reservation.HallId = int.Parse(parts[0]);
                reservation.MovieId = int.Parse(parts[1]);
            }

            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reîncarcă opțiunile în caz de eroare
            var hallMovieOptions = _context.HallMovies
                .Include(hm => hm.Hall)
                .Include(hm => hm.Movie)
                .Select(hm => new SelectListItem
                {
                    Value = $"{hm.HallId}|{hm.MovieId}",
                    Text = $"{hm.Movie.Title} - {hm.Hall.HallName} ({hm.StartDate:dd.MM.yyyy HH:mm})"
                }).ToList();

            ViewBag.HallMovieOptions = hallMovieOptions;

            return View(reservation);
        }


        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["HallId"] = new SelectList(_context.Halls, "HallId", "HallId", reservation.HallId);
            ViewData["HallId"] = new SelectList(_context.HallMovies, "HallId", "HallId", reservation.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", reservation.MovieId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,FirstName,LastName,PhoneNumber,SeatNumber,HallId,MovieId,ReservationDate")] Reservation reservation)
        {
            if (id != reservation.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationId))
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
            ViewData["HallId"] = new SelectList(_context.Halls, "HallId", "HallId", reservation.HallId);
            ViewData["HallId"] = new SelectList(_context.HallMovies, "HallId", "HallId", reservation.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", reservation.MovieId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Hall)
                .Include(r => r.HallMovie)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationId == id);
        }
    }
}

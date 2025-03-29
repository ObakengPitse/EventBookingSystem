using EventBookingSystem.Data;
using EventBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        // POST: Bookings/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("EventId,BookingDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Check if there are any conflicting bookings
                var existingBooking = await _context.Booking
                    .FirstOrDefaultAsync(b => b.EventId == booking.EventId);

                if (existingBooking != null)
                {
                    ModelState.AddModelError("", "This event is already booked.");
                    return View(booking);
                }

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }
    }
}

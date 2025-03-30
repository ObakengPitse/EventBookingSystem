using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventBookingSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventBookingSystem.Data;

public class BookingController : Controller
{
    private readonly AppDbContext _context;

    public BookingController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Bookings - Display list of bookings
    public async Task<IActionResult> Index()
    {
        var bookings = await _context.Booking
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .Include(b => b.Customer)
            .ToListAsync();
        return View(bookings);
    }

    // GET: Create - Show form to create a new booking
    public IActionResult Create()
    {
        ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName");
        ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName");
        ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName"); // Fixed Customer dropdown
        return View();
    }

    // POST: Create - Handle form submission to create a new booking
    [HttpPost]
    public async Task<IActionResult> Create([Bind("BookingId,EventId,VenueId,CustomerId,BookingDate")] Booking booking)
    {
        // Check for conflicts in bookings for the same venue and event date
        var conflictingBooking = await _context.Booking
            .Include(b => b.Event)
            .Where(b => b.VenueId == booking.VenueId && b.Event.EventDate == booking.BookingDate)
            .FirstOrDefaultAsync();

        if (conflictingBooking != null)
        {
            throw new ArgumentException("Booking Conflict"); 
        }

        _context.Add(booking);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Edit - Display form to edit an existing booking
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var booking = await _context.Booking
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .Include(b => b.Customer)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking == null)
        {
            return NotFound();
        }

        // Populate dropdowns
        ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
        ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
        ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName", booking.CustomerId);

        return View(booking);
    }
    // POST: Edit - Handle form submission to update an existing booking
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,VenueId,CustomerId,BookingDate")] Booking booking)
    {
        if (id != booking.BookingId)
        {
            return NotFound();
        }

        try
        {
            // Check for booking conflicts after editing
            var conflictingBooking = await _context.Booking
                .Include(b => b.Event)
                .Where(b => b.VenueId == booking.VenueId && b.Event.EventDate.Date == booking.BookingDate.Date && b.BookingId != booking.BookingId)
                .FirstOrDefaultAsync();

            if (conflictingBooking != null)
            {
                ModelState.AddModelError("", "This venue is already booked for the selected date.");
                ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
                ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
                ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName", booking.CustomerId);
                return View(booking);
            }

            // Update booking if no conflicts
            _context.Update(booking);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookingExists(booking.BookingId))
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

    // Delete: Show form to confirm deletion of booking
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var booking = await _context.Booking
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .Include(b => b.Customer)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking == null)
        {
            return NotFound();
        }

        return View(booking);
    }

    // Delete: Handle booking deletion
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var booking = await _context.Booking.FindAsync(id);
        _context.Booking.Remove(booking);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookingExists(int id)
    {
        return _context.Booking.Any(b => b.BookingId == id);
    }

    // Helper to populate dropdowns for Create/Edit views
    private void PopulateDropdowns(Booking booking)
    {
        ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
        ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
        ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName", booking.CustomerId);
    }
}
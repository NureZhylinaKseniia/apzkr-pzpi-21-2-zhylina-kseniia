using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.Models;
using Application.DBContext;

namespace Application.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Booking> GetAllBookings()
        {
            return _context.Booking.Include(k => k.User).Include(k => k.Workplaces).AsQueryable();
        }

        public Booking GetBookingById(int id)
        {
            return _context.Booking.Include(k => k.User).Include(k => k.Workplaces).FirstOrDefault(k => k.BookingId == id);
        }

        public IEnumerable<Booking> GetBookingsByUserId(int userId)
        {
            return _context.Booking
                    .Where(booking => booking.User.UserId == userId)
                    .ToList();
        }

        public IEnumerable<Booking> GetBookingByWorkplaceId(int workplaceId)
        {
            var bookings = GetAllBookings();

            List<Booking> result = new List<Booking>();

            foreach (var booking in bookings) 
            {
                if(booking.Workplaces.Select(x => x.WorkplaceId).Contains(workplaceId))
                {
                    result.Add(booking);
                }
            }

            return result;
        }

        public void CreateBooking(Booking booking)
        {
            _context.Booking.Add(booking);
            _context.SaveChanges();
        }

        public void UpdateBooking(Booking booking)
        {
            _context.Entry(booking).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteBooking(int id)
        {
            var booking = _context.Booking.Find(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
                _context.SaveChanges();
            }
        }
    }
}

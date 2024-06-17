using Application.Models;

namespace Application.Repositories
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetAllBookings();
        Booking GetBookingById(int id);
        IEnumerable<Booking> GetBookingsByUserId(int userId);
        IEnumerable<Booking> GetBookingByWorkplaceId(int workplaceId);
        void CreateBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(int id);
    }
}

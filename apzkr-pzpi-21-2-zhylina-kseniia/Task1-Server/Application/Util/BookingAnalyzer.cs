using Application.Models;
using Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Application.Util
{
    class TimeRange
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }

    public class BookingAnalyzer
    {

        private readonly IRoomRepository _roomRepository;
        private readonly IWorkplaceRepository _workplaceRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookingAnalyzer(IRoomRepository roomRepository, IWorkplaceRepository workplaceRepository, IBookingRepository bookingRepository)
        {
            _roomRepository = roomRepository;
            _workplaceRepository = workplaceRepository;
            _bookingRepository = bookingRepository;
        }

        public List<Booking> GetAllBookingsInThisWorkplace(int id)
        {
            var rooms = _roomRepository.GetRoomsByCoworkingSpaceId(id);
            if (rooms == null) { return null; }

            List<Workplace> allWorkplaces = new List<Workplace>();
            foreach (var room in rooms)
            {
                var workplaces = _workplaceRepository.GetWorkplacesByRoomId(room.RoomId);
                allWorkplaces.AddRange(workplaces);
            }

            if (allWorkplaces.Count == 0) { return null; }

            List<Booking> allBookings = new List<Booking>();
            foreach (var workplace in allWorkplaces)
            {
                var booking = _bookingRepository.GetBookingByWorkplaceId(workplace.WorkplaceId);
                allBookings.AddRange(booking);
            }

            return allBookings;
        }

        static int CountOverlaps(List<TimeRange> ranges, TimeSpan time)
        {
            int count = 0;
            foreach (var range in ranges)
            {
                if (time >= range.Start && time <= range.End)
                {
                    count++;
                }
            }
            return count;
        }

        public (TimeSpan start, TimeSpan end) GetMostPopularTimePeriod(int id)
        {
            var bookings = GetAllBookingsInThisWorkplace(id);

            List<TimeRange> timeRanges = new List<TimeRange>();

            foreach (var booking in bookings)
            {
                timeRanges.Add(new TimeRange { Start = booking.StartDateTime.TimeOfDay, End = booking.EndDateTime.TimeOfDay });
            }

            TimeSpan startTime = TimeSpan.FromHours(0);
            TimeSpan endTime = TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(59)); // 23:59
            int maxOverlaps = 0;
            TimeSpan maxOverlapStartTime = startTime;
            TimeSpan maxOverlapEndTime = startTime;

            for (TimeSpan time = startTime; time <= endTime; time = time.Add(TimeSpan.FromMinutes(1)))
            {
                int overlaps = CountOverlaps(timeRanges, time);
                if (overlaps > maxOverlaps)
                {
                    maxOverlaps = overlaps;
                    maxOverlapStartTime = time;
                    maxOverlapEndTime = time.Add(TimeSpan.FromMinutes(59)); // Considering a one-hour window
                }
            }
            return (maxOverlapStartTime, maxOverlapEndTime);
        }
    }
}

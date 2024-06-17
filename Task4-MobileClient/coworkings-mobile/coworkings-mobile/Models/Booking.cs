using System;

namespace coworkings_mobile.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int BookingCode { get; set; }
        public int UserId { get; set; }
        public int WorkplaceId { get; set; }

        // Formatted fields
        public string FormattedStartDate => StartDateTime.ToString("dd-MM-yyyy");
        public string FormattedStartTime => StartDateTime.ToString("HH:mm");
        public string FormattedEndDate => EndDateTime.ToString("dd-MM-yyyy");
        public string FormattedEndTime => EndDateTime.ToString("HH:mm");

        // Determine if the booking is currently valid
        public bool IsValid => DateTime.UtcNow < EndDateTime.ToUniversalTime() && DateTime.UtcNow > StartDateTime.ToUniversalTime();
    }
}

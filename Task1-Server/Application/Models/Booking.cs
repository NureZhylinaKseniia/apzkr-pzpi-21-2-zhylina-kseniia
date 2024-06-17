using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int BookingCode { get; set; }
    
        [ForeignKey("UserId")]
        public User User { get; set; }

        public List<Workplace> Workplaces { get; set; }
    }
}
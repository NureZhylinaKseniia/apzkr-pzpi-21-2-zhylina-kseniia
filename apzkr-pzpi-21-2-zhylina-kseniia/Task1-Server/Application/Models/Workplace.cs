using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Application.Models
{
    public class Workplace
    {
        public int WorkplaceId { get; set; }
        public float HourlyRate { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        [JsonIgnore]
        public List<Booking> Bookings { get; set; }
    }
}
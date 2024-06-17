using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string Description { get; set; }

        [ForeignKey("CoworkingSpaceId")]
        public CoworkingSpace CoworkingSpace { get; set; }
    }
}
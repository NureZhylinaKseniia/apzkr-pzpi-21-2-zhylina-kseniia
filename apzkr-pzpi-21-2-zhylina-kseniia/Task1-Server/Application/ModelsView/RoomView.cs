using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.Models;

namespace Application.ViewModels
{
    [NotMapped]
    public class RoomView
    {
        public string RoomName { get; set; }
        public string Description { get; set; }
        public int CoworkingSpaceId { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.Models;

namespace Application.ViewModels
{
    [NotMapped]
    public class WorkplaceView
    {
        public float HourlyRate { get; set; }
        public int RoomId { get; set; }
    }
}
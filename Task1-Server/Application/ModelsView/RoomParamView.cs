using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.Models;

namespace Application.ViewModels
{
    [NotMapped]
    public class RoomParamView
    {
        public string? RoomName { get; set; }
        public float? HourlyRate { get; set; }
        public string? CoworkingSpaceName { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.Models;

namespace Application.ViewModels
{

    [NotMapped]
    public class BookingView
    {
        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDateTime { get; set; }

        public int UserId { get; set; }

        public List<int> WorkplacesId { get; set; }
    }
}

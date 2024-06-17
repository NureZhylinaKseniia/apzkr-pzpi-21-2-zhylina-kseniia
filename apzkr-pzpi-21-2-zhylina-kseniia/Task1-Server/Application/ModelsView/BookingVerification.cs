using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Application.ModelsView
{
    public class BookingVerification
    {
        public int code { get; set; }
        public DateTime timestamp { get; set; }
    }
}
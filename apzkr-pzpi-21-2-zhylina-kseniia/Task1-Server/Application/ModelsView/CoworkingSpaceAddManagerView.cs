using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.ViewModels
{
    [NotMapped]
    public class CoworkingSpaceAddManagerView
    {
        public int CoworkingSpaceId { get; set; }
        public int ManagerId { get; set; }
    }
}
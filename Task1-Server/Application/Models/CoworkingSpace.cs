using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class CoworkingSpace
    {
        public int CoworkingSpaceId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        [ForeignKey("ManagerId")]
        public Manager? Manager { get; set; }
    }
}
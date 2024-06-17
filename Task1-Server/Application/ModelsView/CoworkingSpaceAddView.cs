﻿using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.ViewModels
{
    [NotMapped]
    public class CoworkingSpaceAddView
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }
}
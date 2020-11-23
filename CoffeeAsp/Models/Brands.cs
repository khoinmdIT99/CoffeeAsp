using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeAsp.Models
{
    public class Brands
    {
        [Key]
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile Photo { get; set; }
    }
}

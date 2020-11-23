using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeAsp.Models
{
    public class Workers
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string Image { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Tymblr { get; set; }
        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile Photo { get; set; }
    }
}

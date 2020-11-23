using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeAsp.Models
{
    public class Counters
    {
        [Key]
        public int Id { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

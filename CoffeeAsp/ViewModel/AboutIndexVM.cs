using CoffeeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeAsp.ViewModel
{
    public class AboutIndexVM
    {
        public IEnumerable<Gallery> galleries { get; set; }
        public IEnumerable<Workers> workers { get; set; }
        public IEnumerable<Counters> counters { get; set; }
        public IEnumerable<Subcribes> subcribes { get; set; }
        public IEnumerable<ColdMenu> coldMenus { get; set; }
        public IEnumerable<HotMenu> hotMenus { get; set; }
        public IEnumerable<Products> products { get; set; }
    }
}

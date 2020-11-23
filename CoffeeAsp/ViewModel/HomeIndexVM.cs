using CoffeeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeAsp.ViewModel
{
    public class HomeIndexVM
    {
        public IEnumerable<Sliders> sliders { get; set; }
        public IEnumerable<Brands> brands { get; set; }
        public IEnumerable<ColdMenu> coldMenus { get; set; }
        public IEnumerable<HotMenu> hotMenus { get; set; }
    }
}

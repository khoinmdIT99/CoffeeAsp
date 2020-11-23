using CoffeeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeAsp.ViewModel
{
    public class MenuIndexVM
    {
        public IEnumerable<HotMenu> hotMenus { get; set; }
        public IEnumerable<ColdMenu> coldMenus { get; set; }
    }
}

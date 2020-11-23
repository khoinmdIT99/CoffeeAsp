using CoffeeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeAsp.ViewModel
{
    public class UserAddRoleVM
    {
        public AppUser AppUser { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

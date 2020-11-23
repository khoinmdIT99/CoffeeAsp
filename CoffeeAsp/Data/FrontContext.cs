using CoffeeAsp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeAsp.Data
{
    public class FrontContext:IdentityDbContext<AppUser>
    {
        public FrontContext(DbContextOptions<FrontContext> options):base(options)
        {

        }
        public DbSet<Sliders> Sliders { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Workers> Workers { get; set; }
        public DbSet<HotMenu> HotMenus { get; set; }
        public DbSet<ColdMenu> ColdMenus { get; set; }
        public DbSet<Subcribes> Subcribes { get; set; }
        public DbSet<Counters> Counters { get; set; }
    }
}

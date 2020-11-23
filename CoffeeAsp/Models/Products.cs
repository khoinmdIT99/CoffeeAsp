using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeAsp.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool HasDiscount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile Photo { get; set; }
        public static implicit operator Carts(Products p)
        {
            return new Carts
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                HasDiscount = p.HasDiscount,
                DiscountPrice = p.DiscountPrice,
                Image = p.Image,
                Quantity = 1
            };
        }
    }
}

﻿using System.ComponentModel.DataAnnotations;
using BookstoreWeb.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = "Price 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Price 51-100")]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public string ImageURL { get; set; }
    }
}

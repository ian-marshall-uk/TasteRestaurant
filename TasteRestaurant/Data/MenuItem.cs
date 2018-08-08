using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TasteRestaurant.Data
{
    public class MenuItem
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string Image { get; set; }

        public string Spicyness { get; set; }

        public enum ESpicy { Mild = 0, Moderate = 1, Spicy = 2}

        [Range( 1, int.MaxValue, ErrorMessage = "Price should be greater than £{1}")]
        public double Price { get; set; }

        [Display(Name = "Category type")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryType CategoryType { get; set; } // Virtual means it will not be added to the db by the migration

        [Display(Name = "Food type")]
        public int FoodTypeId { get; set; }

        [ForeignKey("FoodTypeId")]
        public virtual FoodType FoodType { get; set; } // Virtual means it will not be added to the db by the migration

    }
}

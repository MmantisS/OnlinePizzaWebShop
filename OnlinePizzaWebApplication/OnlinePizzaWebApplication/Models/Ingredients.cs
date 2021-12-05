﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlinePizzaWebApplication.Models
{
    public class Ingredients
    {
        public Ingredients()
        {
            PizzaIngredients = new HashSet<PizzaIngredients>();
        }

        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Name should only include letters and number.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<PizzaIngredients> PizzaIngredients { get; set; }

        public DateTime ExpirationDate { get; set; }
        public double Price { get; set; }

        public double PurchasePrice { get; set; }
        public int Quantity { get; set; }

    }
}
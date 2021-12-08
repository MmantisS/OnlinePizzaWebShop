using System;
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Название")]
        public string Name { get; set; }

        public virtual ICollection<PizzaIngredients> PizzaIngredients { get; set; }
        [DisplayName("Срок годности")]
        public DateTime ExpirationDate { get; set; }
        [DisplayName("Цена")]
        public double Price { get; set; }
        [DisplayName("Закупочная цена")]
        public double PurchasePrice { get; set; }
        [DisplayName("Количество")]
        public int Quantity { get; set; }

    }
}
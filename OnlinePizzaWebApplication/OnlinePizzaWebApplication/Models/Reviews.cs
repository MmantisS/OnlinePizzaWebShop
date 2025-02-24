﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlinePizzaWebApplication.Models
{
    public class Reviews
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Title should only include letters and number.")]
        [DataType(DataType.Text)]
        [DisplayName("Название")]
        [Required]
        public string Title { get; set; }

        [StringLength(500, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [Required]
        [DisplayName("Описание")]
        public string Description { get; set; }

        [Range(1, 5)]
        [DisplayName("Оценка")]
        public int Grade { get; set; }
        [DisplayName("Дата")]
        public DateTime Date { get; set; }

        [DisplayName("Выберите пиццу")]
        public int PizzaId { get; set; }

        public virtual Pizzas Pizza { get; set; }

        public string UserId { get; set; }
        [DisplayName("Пользователь")]
        public IdentityUser User { get; set; }

    }
}
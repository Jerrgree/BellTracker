﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Price
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [InverseProperty("Prices")]
        [ForeignKey("WeekId")]
        public Week Week { get; set; }

        public int WeekId { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public bool IsMorning { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
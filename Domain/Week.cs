using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Week
    {
        public Week()
        {
            Prices = new HashSet<Price>();
        }

        [Key]
        public int Id { get; set; }

        [Required, StringLength(4, MinimumLength = 4)]
        public string Year { get; set; }

        [Required]
        public int WeekNumber { get; set; }

        [InverseProperty("Week")]
        public virtual ICollection<Price> Prices { get; set; }
    }
}

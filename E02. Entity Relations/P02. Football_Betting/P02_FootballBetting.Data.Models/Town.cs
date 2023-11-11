using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Town
    {
        [Key]
        public int TownId { get; set; }

        public string Name { get; set; } = null!;

        [Required]
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]

        public virtual Country Country { get; set; } = null!;

        public ICollection<Team> Teams { get; set; } = null!;
    }
}

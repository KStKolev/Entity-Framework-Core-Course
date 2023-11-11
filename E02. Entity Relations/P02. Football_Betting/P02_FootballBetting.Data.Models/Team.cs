using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string LogoUrl { get; set; } = null!;

        public string Initials { get; set; } = null!;

        [Required]
        public decimal Budget { get; set; }

        public int PrimaryKitColorId { get; set; }
        [ForeignKey(nameof(PrimaryKitColorId))]
        public virtual Color PrimaryKitColor { get; set; } = null!;

        public int SecondaryKitColorId { get; set; }
        public virtual Color SecondaryKitColor { get; set; } = null!;

        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]

        public virtual Town Town { get; set; } = null!;

        [InverseProperty(nameof(Game.HomeTeam))]
        public virtual ICollection<Game> HomeGames { get; set; } = null!;

        [InverseProperty(nameof(Game.AwayTeam))]
        public virtual ICollection<Game> AwayGames { get; set; } = null!;

        public ICollection<Player> Players { get; set; } = null!;
    }
}

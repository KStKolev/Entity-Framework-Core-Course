using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        public int HomeTeamId { get; set; }
        [ForeignKey(nameof(HomeTeamId))]
        public virtual Team HomeTeam { get; set; } = null!;

        public int AwayTeamId { get; set; }
        [ForeignKey(nameof(AwayTeamId))]
        public virtual Team AwayTeam { get; set; } = null!;

        [Required]
        public int HomeTeamGoals { get; set; }

        [Required]
        public int AwayTeamGoals { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public double HomeTeamBetRate { get; set; }

        [Required]
        public double AwayTeamBetRate { get; set; }

        [Required]
        public double DrawBetRate { get; set; }

        public string Result { get; set; } = null!;

        public ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;

        public ICollection<Bet> Bets { get; set; } = null!;
    }
}

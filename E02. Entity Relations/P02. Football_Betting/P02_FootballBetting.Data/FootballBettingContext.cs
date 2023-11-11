using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext(DbContextOptions options) : base(options) { }

        private const string _connection = "Server=.\\SQLEXPRESS;Database=FootballBookmakerSystem;Integrated Security=true;";

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Town> Towns { get; set; } = null!;
        public DbSet<Color> Colors { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Bet> Bets { get; set; } = null!;
        public DbSet<PlayerStatistic> PlayersStatistics { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(ps => new { ps.PlayerId, ps.GameId });
        }
    }
}

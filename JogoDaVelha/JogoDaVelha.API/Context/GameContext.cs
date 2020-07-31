using JogoDaVelha.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace JogoDaVelha.API.Context
{
    public class GameContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

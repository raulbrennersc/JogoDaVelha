using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace JogoDaVelha.API.Interfaces
{
    public interface IGameService
    {
        DbSet<Game> Games { get; set; }
        Game Create(int gameSize);
        Game Get(string id);
        void Move(Game game, MovementDto movement);
        void Result(Game game);
    }
}

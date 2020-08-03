using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;

namespace JogoDaVelha.API.Interfaces
{
    public interface IGameService
    {
        Game Create(int gameSize);
        Game Get(string id);
        void Move(Game game, MovementDto movement);
        void Result(Game game);
    }
}

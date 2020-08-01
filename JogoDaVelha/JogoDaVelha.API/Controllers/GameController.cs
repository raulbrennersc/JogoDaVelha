using JogoDaVelha.API.Context;
using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
using JogoDaVelha.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace JogoDaVelha.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameContext _gameContext;
        private readonly int GameSize;
        public GameController(GameContext gameContext, IConfiguration config)
        {
            _gameContext = gameContext;
            GameSize = int.Parse(config.GetSection("AppSettings:GameSize").Value);
        }

        [HttpPost]
        public async Task <IActionResult> CreateGame()
        {
            var newGame = new Game(GameSize);
            _gameContext.Set<Game>().Add(newGame);
            await _gameContext.SaveChangesAsync();
            var response = new { newGame.Id, FirstPlayer = newGame.NextPlayer };
            return Ok(response);
        }

        [HttpPost("{id}/movement")]
        public async Task <IActionResult> Movement(string id, MovementDto movement)
        {
            var guid = new Guid(id);
            var game = _gameContext.Set<Game>().Find(guid);

            if (game == null)
            {
                return BadRequest(new MoveResultDto { Msg = "Partida não encontrada." });
            }

            if (game.Winner != null)
            {
                return BadRequest(new MoveResultDto { Msg = "Partida finalizada.", Winner = game.Winner });
            }

            try
            {
                var gameHelper = new GameService(GameSize);
                gameHelper.Move(game, movement);
                gameHelper.Result(game);
                _gameContext.Set<Game>().Update(game);
                await _gameContext.SaveChangesAsync();
                if (game.Winner != null )
                {
                    return Ok(new MoveResultDto { Msg = "Partida finalizada.", Winner = game.Winner });
                }

                return Ok(new MoveResultDto { Msg = "Jogada efetuada com sucesso." });
            }
            catch (Exception e)
            {
                return BadRequest(new MoveResultDto { Msg = e.Message });
            }
        }
    }
}

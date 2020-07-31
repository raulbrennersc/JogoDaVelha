using JogoDaVelha.API.Context;
using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
using JogoDaVelha.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

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
        public ActionResult CreateGame()
        {
            var newGame = new Game(GameSize);
            _gameContext.Set<Game>().Add(newGame);
            _gameContext.SaveChanges();
            var response = new { newGame.Id, FirstPlayer = newGame.NextPlayer };
            return Ok(response);
        }

        [HttpPost("{id}/movement")]
        public ActionResult Movement(string id, MovementDto movement)
        {
            var guid = new Guid(id);
            var game = _gameContext.Set<Game>().Find(guid);

            if (game == null)
            {
                return BadRequest(new MoveResultDto { Msg = "Partida não encontrada." });
            }

            if (!string.IsNullOrEmpty(game.Winner))
            {
                return BadRequest(new MoveResultDto { Msg = "Partida finalizada.", Winner = game.Winner });
            }

            try
            {
                var gameHelper = new GameHelper(GameSize);
                gameHelper.Move(game, movement);
                gameHelper.Result(game);
                _gameContext.Set<Game>().Update(game);
                _gameContext.SaveChangesAsync();
                if (!string.IsNullOrEmpty(game.Winner))
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

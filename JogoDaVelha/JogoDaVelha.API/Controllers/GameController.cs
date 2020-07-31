using JogoDaVelha.API.Context;
using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
using JogoDaVelha.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JogoDaVelha.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameContext _gameContext;
        public GameController(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        [HttpPost]
        public ActionResult CreateGame()
        {
            var newGame = new Game();
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

            if(game == null)
            {
                return BadRequest(new MoveResultDto { Msg = "Partida não encontrada." });
            }
            if (!string.IsNullOrEmpty(game.Winner))
            {
                return BadRequest(new MoveResultDto { Msg = "Partida finalizada.", Winner = game.Winner });
            }

            try
            {
                GameHelper.Move(game, movement);
                GameHelper.Result(game);
                if (!string.IsNullOrEmpty(game.Winner))
                {
                    return Ok(new MoveResultDto { Msg = "Partida finalizada.", Winner = game.Winner });
                }

                return Ok(new MoveResultDto { Msg = "Jogada efetuada com sucesso."});
            }
            catch(Exception e)
            {
                return BadRequest(new MoveResultDto { Msg = e.Message });
            }
        }
    }
}

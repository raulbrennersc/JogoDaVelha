using JogoDaVelha.API.Context;
using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
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

            object result;
            if(game == null)
            {
                result = Ok(new MoveResultDto { Msg = "Partida não encontrada." });
            }

            return Ok();
        }
    }
}

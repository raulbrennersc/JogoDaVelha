using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace JogoDaVelha.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly int _gameSize;
        private readonly IGameService _gameService;
        public GameController(IGameService gameService, IConfiguration config)
        {
            _gameSize = int.Parse(config.GetSection("AppSettings:GameSize").Value);
            _gameService = gameService;
        }

        [HttpPost]
        public ActionResult CreateGame()
        {
            var newGame = _gameService.Create(_gameSize);
            var response = new { newGame.Id, FirstPlayer = newGame.NextPlayer };
            return Created($"game/{newGame.Id}", response);
        }

        [HttpPost("{id}/movement")]
        public ActionResult Movement(string id, MovementDto movement)
        {
            var game = _gameService.Get(id);

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
                _gameService.Move(game, movement);
                _gameService.Result(game);
                if (game.Winner != null)
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

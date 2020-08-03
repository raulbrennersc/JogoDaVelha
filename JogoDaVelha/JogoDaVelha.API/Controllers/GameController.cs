using JogoDaVelha.API.Context;
using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
using JogoDaVelha.API.Interfaces;
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
        private readonly int _gameSize;
        private readonly IGameService _gameService;
        public GameController(GameContext gameContext, IGameService gameService, IConfiguration config)
        {
            _gameContext = gameContext;
            _gameSize = int.Parse(config.GetSection("AppSettings:GameSize").Value);
            _gameService = gameService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame()
        {
            var newGame = _gameService.Create(_gameSize);
            await _gameContext.SaveChangesAsync();
            var response = new { newGame.Id, FirstPlayer = newGame.NextPlayer };
            return Ok(response);
        }

        [HttpPost("{id}/movement")]
        public async Task<IActionResult> Movement(string id, MovementDto movement)
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
                _gameContext.Set<Game>().Update(game);
                await _gameContext.SaveChangesAsync();
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

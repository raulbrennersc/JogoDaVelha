using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
using JogoDaVelha.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JogoDaVelha.UnitTest
{
    public class FakeGameService : IGameService
    {
        public List<Game> Games { get; set; }
        public FakeGameService()
        {
            Games = new List<Game>
            {
                new Game{ Id = new Guid("a599cf32-676d-47fa-b8ad-c3f335f9a78a"), Matrix = "XXX--O-O-", NextPlayer = 'O'},
                new Game{ Id = new Guid("08954715-faa1-4b02-a979-c73ac7c1745b"), Matrix = "XOOXOXO--", NextPlayer = 'X'},
            };
        }
        public Game Create(int gameSize)
        {
            var matrix = string.Concat(Enumerable.Repeat("-", gameSize));
            var newGame = new Game
            {
                Id = new Guid("f30d0705-5cf6-4f39-9576-11f5dae23d59"),
                Matrix = matrix,
                NextPlayer = 'X'
            };
            Games.Add(newGame);
            return newGame;
        }

        public Game Get(string id)
        {
            return Games.FirstOrDefault(g => g.Id == new Guid(id));
        }

        public void Move(Game game, MovementDto movement)
        {
            throw new NotImplementedException();
        }

        public void Result(Game game)
        {
            throw new NotImplementedException();
        }
    }
}

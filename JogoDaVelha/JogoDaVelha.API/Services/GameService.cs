using JogoDaVelha.API.Context;
using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
using JogoDaVelha.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace JogoDaVelha.API.Helpers
{
    public class GameService : IGameService
    {
        private GameContext GameContext { get; set; }
        public DbSet<Game> Games { get; set; }

        public GameService(GameContext context)
        {
            Games = context.Games;
            GameContext = context;
        }

        public Game Get(string id)
        {
            return Games.Find(new Guid(id));
        }

        public Game Create(int gameSize)
        {
            var newGame = new Game(gameSize);
            Games.Add(newGame);
            GameContext.SaveChanges();
            return newGame;
        }

        public void Move(Game game, MovementDto movement)
        {
            var matrix = StringToMatrix(game.Matrix);
            var x = movement.Position.X;
            var y = movement.Position.Y;
            var lastIndex = matrix.Length - 1;
            if (x > lastIndex || x < 0 || y > lastIndex || y < 0 || matrix[lastIndex - y][x] != '-')
            {
                throw new Exception("Jogada inválida.");
            }
            else if (game.NextPlayer != movement.Player)
            {
                throw new Exception("Não é o turno do jogador.");
            }
            else
            {
                matrix[lastIndex - y][x] = game.NextPlayer;
                game.NextPlayer = game.NextPlayer == 'X' ? 'O' : 'X';
                game.Matrix = MatrixToString(matrix);
                Games.Update(game);
                GameContext.SaveChanges();

            }
        }

        public void Result(Game game)
        {
            var lastPlayer = game.NextPlayer == 'X' ? 'O' : 'X';
            var matrix = StringToMatrix(game.Matrix);
            var i = 0;
            var draw = true;
            //Vericiando linhas e colunas
            while (game.Winner == null && i < matrix.Length)
            {
                var line = matrix[i];
                var column = matrix.Select(a => a[i]);
                if (line.All(c => c == lastPlayer) || column.All(c => c == lastPlayer))
                {
                    game.Winner = lastPlayer.ToString();
                }

                if (!line.Contains('X') || !line.Contains('O') || !column.Contains('X') || !column.Contains('O'))
                {
                    draw = false;
                }
                i++;
            }

            //Verificando diagonais
            if (game.Winner == null)
            {
                var diag1 = new char[matrix.Length];
                var diag2 = new char[matrix.Length];
                for (i = 0; i < matrix.Length; i++)
                {
                    diag1[i] = matrix[i][i];
                    diag2[i] = matrix[i][matrix.Length - 1 - i];
                }

                if (diag1.All(c => c == lastPlayer) || diag2.All(c => c == lastPlayer))
                {
                    game.Winner = lastPlayer.ToString();
                }

                if (!diag1.Contains('X') || !diag1.Contains('O') || !diag2.Contains('X') || !diag2.Contains('O'))
                {
                    draw = false;
                }
            }

            if (draw)
            {
                game.Winner = draw ? "Draw" : null;
            }
            if (game.Winner != null)
            {
                Games.Update(game);
                GameContext.SaveChanges();
            }
        }

        private string MatrixToString(char[][] matrix)
        {
            var str = "";
            foreach (var array in matrix)
            {
                str += string.Concat(array);
            }
            return str;
        }

        private char[][] StringToMatrix(string str)
        {
            var size = (int)Math.Sqrt(str.Length);
            char[][] matrix = new char[size][];
            var charArr = str.ToCharArray();
            for (int i = 0; i < size; i++)
            {
                matrix[i] = charArr.Skip(size * i).Take(size).ToArray();
            }
            return matrix;
        }

        public void PrintGame(Game game)
        {
            var matrix = StringToMatrix(game.Matrix);
            foreach (var arrX in matrix)
            {
                System.Diagnostics.Debug.WriteLine(string.Concat(arrX));
            }
        }
    }
}

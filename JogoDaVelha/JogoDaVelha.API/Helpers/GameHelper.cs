using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace JogoDaVelha.API.Helpers
{
    public static class GameHelper
    {
        private static string MatrixToString(char[][] matrix)
        {
            var str = "";
            foreach (var array in matrix)
            {
                str += string.Concat(array);
            }
            return str;
        }

        private static char[][] StringToMatrix(string str)
        {
            char[][] matrix = new char[3][];
            var charArr = str.ToCharArray();
            matrix[0] = charArr.Take(3).ToArray();
            matrix[1] = charArr.Skip(3).Take(3).ToArray();
            matrix[2] = charArr.Skip(6).Take(3).ToArray();

            return matrix;
        }

        public static void Move(Game game, MovementDto movement)
        {
            var matrix = StringToMatrix(game.Matrix);
            var x = movement.Position.X;
            var y = movement.Position.Y;
            if (x > 2 || x < 0 || y > 2 || y < 0 || matrix[2-y][x] != '-')
            {
                throw new Exception("Jogada inválida.");
            }
            else if(game.NextPlayer != movement.Player)
            {
                throw new Exception("Não é o turno do jogador.");
            }
            else
            {
                matrix[2-y][x] = game.NextPlayer;
                game.NextPlayer = game.NextPlayer == 'X' ? 'O' : 'X';
                game.Matrix = MatrixToString(matrix);
            }
        }

        public static void Result(Game game)
        {
            var lastPlayer = game.NextPlayer == 'X' ? 'O' : 'X';
            var matrix = StringToMatrix(game.Matrix);
            var draw = true;
            for (int i = 0; i < 3; i++)
            {
                var line = matrix[i];
                var column = matrix.Select(a => a[i]);
                if (line.All(c => c == lastPlayer) || column.All(c => c == lastPlayer))
                {
                    game.Winner = lastPlayer.ToString();
                    return;
                }

                if(line.Contains('-') || column.Contains('-'))
                {
                    draw = false;
                }
            }

            if (draw)
            {
                game.Winner = "Draw";
            }
        }

        public static void PrintGame(Game game)
        {
            var matrix = StringToMatrix(game.Matrix);
            foreach (var arrX in matrix)
            {
                System.Diagnostics.Debug.WriteLine(string.Concat(arrX));
            }
        }
    }
}

using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JogoDaVelha.API.Helpers
{
    public static class GameHelper
    {
        private static string ArrayToString(char[][] array)
        {
            var str = "";
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    str += array[i][j];
                }
            }
            return str;
        }

        private static char[][] StringToArray(string str)
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
            var matrix = StringToArray(game.Matrix);
            var x = movement.Position.X;
            var y = movement.Position.Y;
            if (x > 2 || x < 0 || y > 2 || y < 0 || matrix[x][y] != '-')
            {
                throw new Exception("Jogada inválida.");
            }
            else if(game.NextPlayer != movement.Player)
            {
                throw new Exception("Não é o turno do jogador.");
            }
            else
            {
                matrix[x][y] = game.NextPlayer;
                game.NextPlayer = game.NextPlayer == 'X' ? 'O' : 'X';
                game.Matrix = ArrayToString(matrix);
            }
        }

        public static void Result(Game game)
        {
            var matrix = StringToArray(game.Matrix);
            game.Winner = "Draw";
        }
    }
}

﻿using JogoDaVelha.API.Dtos;
using JogoDaVelha.API.Entities;
using System;
using System.Linq;

namespace JogoDaVelha.API.Helpers
{
    public class GameHelper
    {
        private readonly int GameSize;
        public GameHelper(int gameSize)
        {
            GameSize = gameSize;
        }

        public string MatrixToString(char[][] matrix)
        {
            var str = "";
            foreach (var array in matrix)
            {
                str += string.Concat(array);
            }
            return str;
        }

        public char[][] StringToMatrix(string str)
        {
            char[][] matrix = new char[GameSize][];
            var charArr = str.ToCharArray();
            for (int i = 0; i < GameSize; i++)
            {
                matrix[i] = charArr.Skip(GameSize * i).Take(GameSize).ToArray();
            }
            return matrix;
        }

        public void Move(Game game, MovementDto movement)
        {
            var matrix = StringToMatrix(game.Matrix);
            var x = movement.Position.X;
            var y = movement.Position.Y;
            var lastIndex = GameSize - 1;
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
            }
        }

        public void Result(Game game)
        {
            var lastPlayer = game.NextPlayer == 'X' ? 'O' : 'X';
            var matrix = StringToMatrix(game.Matrix);
            if (VerifyWinner(matrix, lastPlayer))
            {
                game.Winner = lastPlayer.ToString();
            }
            else if(matrix.All(a => a.All(c => c != '-')))
            {
                game.Winner = "Draw";
            }
        }

        public bool VerifyWinner(char[][] matrix, char lastPlayer)
        {
            //Vericiando linhas e colunas
            for (int i = 0; i < GameSize; i++)
            {
                var line = matrix[i];
                var column = matrix.Select(a => a[i]);
                if (line.All(c => c == lastPlayer) || column.All(c => c == lastPlayer))
                {
                    return true;
                }
            }

            //Verificando diagonais
            var diag1 = new char[GameSize];
            var diag2 = new char[GameSize];
            for (int i = 0; i < GameSize; i++)
            {
                diag1[i] = matrix[i][i];
                diag2[i] = matrix[i][GameSize - 1 - i];
            }

            return diag1.All(c => c == lastPlayer) || diag2.All(c => c == lastPlayer);
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

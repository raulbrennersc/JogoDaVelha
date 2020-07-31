using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JogoDaVelha.API.Entities
{
    public class Game
    {
        public virtual Guid Id { get; set; }
        public virtual string Matrix { get; set; }
        public virtual char NextPlayer { get; set; }

        public Game()
        {
            var rand = new Random().NextDouble();
            NextPlayer = rand > 0.5 ? 'X' : 'O';
            //O EFCore não consegue salvar arrays de tipos primitivos
            //então a matriz que representa o jogo será uma string de tamanho 9 (3x3)
            Matrix = "---------";
        }
    }

}

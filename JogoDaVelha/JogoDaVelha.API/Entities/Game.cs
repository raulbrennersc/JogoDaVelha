using System;

namespace JogoDaVelha.API.Entities
{
    public class Game
    {
        public virtual Guid Id { get; set; }
        public virtual string Matrix { get; set; }
        public virtual char NextPlayer { get; set; }
        public virtual string Winner { get; set; }

        public Game() { }
        public Game(int gameSize)
        {
            var rand = new Random().NextDouble();
            NextPlayer = rand > 0.5 ? 'X' : 'O';
            //O EFCore não consegue salvar arrays de tipos primitivos
            //então a matriz que representa o jogo será uma string de tamanho definido no appsettings.json
            Matrix = "";
            for (int i = 0; i < gameSize * gameSize; i++)
            {
                Matrix += "-";
            }
        }
    }

}

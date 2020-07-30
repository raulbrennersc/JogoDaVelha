using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JogoDaVelha.API.Entities
{
    public class Game
    {
        public virtual char[][] Matrix { get; set; }
        public virtual Guid Id { get; set; }
        public virtual char NextPlayer { get; set; }
    }
}

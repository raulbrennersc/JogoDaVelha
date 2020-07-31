using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JogoDaVelha.API.Dtos
{
    public class MovementDto
    {
        public char Player { get; set; }
        public PositionDto Position { get; set; }
    }
}

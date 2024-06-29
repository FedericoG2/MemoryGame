using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegodeMemoria.Modelos
{
    public class Nivel
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Puntaje { get; set; }
        public int Intentos { get; set; }
        public int JugadorId { get; set; }
    }
}

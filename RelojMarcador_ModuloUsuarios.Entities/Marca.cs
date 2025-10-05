using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelojMarcador_ModuloUsuarios.Entities
{
    public class Marca
    {
        public int ID_Marca { get; set; }
        public int ID_Usuario { get; set; }
        public int ID_Area { get; set; }
        public string Detalle { get; set; } = string.Empty;
        public string Tipo_Marca { get; set; } = string.Empty; //Entrada o Salida
        public DateTime Fecha_Hora { get; set; }
    }
}

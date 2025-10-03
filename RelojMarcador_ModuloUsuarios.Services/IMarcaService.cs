using RelojMarcador_ModuloUsuarios.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RelojMarcador_ModuloUsuarios.Services
{
    public interface IMarcaService
    {
        Task<bool> ValidarFuncionario(string identificacion, string contrasena);
        Task<IEnumerable<Area>> ObtenerAreasPorIdentificacion(string identificacion);
        Task<int> RegistrarMarca(string identificacion, int idArea, string detalle, string tipoMarca);
    }
}


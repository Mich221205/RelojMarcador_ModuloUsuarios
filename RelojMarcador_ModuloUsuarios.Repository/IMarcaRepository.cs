using RelojMarcador_ModuloUsuarios.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RelojMarcador_ModuloUsuarios.Repository
{
    public interface IMarcaRepository
    {
        string ProbarConexion();
        Task<bool> ValidarFuncionario(string identificacion, string contrasena); // ← AQUI
        Task<int> ObtenerIDFuncionario(string identificacion);
        Task<IEnumerable<Area>> ObtenerAreasFuncionario(int idFuncionario);
        Task<int> RegistrarMarca(int idFuncionario, int idArea, string detalle, string tipoMarca);
    }
}


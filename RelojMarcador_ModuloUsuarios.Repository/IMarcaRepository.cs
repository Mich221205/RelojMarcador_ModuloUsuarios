using RelojMarcador_ModuloUsuarios.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RelojMarcador_ModuloUsuarios.Repository
{
    public interface IMarcaRepository
    {
        string ProbarConexion();
        Task<bool> ValidarUsuario(string identificacion, string contrasena); // ← AQUI
        Task<int> ObtenerIDUsuario(string identificacion);
        Task<IEnumerable<Area>> ObtenerAreasUsuario(int idFuncionario);
        Task<int> RegistrarMarca(int idFuncionario, int idArea, string detalle, string tipoMarca);
    }
}


using RelojMarcador_ModuloUsuarios.Entities;
using RelojMarcador_ModuloUsuarios.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RelojMarcador_ModuloUsuarios.Services
{
    public class MarcaService : IMarcaService
    {
        private readonly IMarcaRepository _repo;

        public MarcaService(IMarcaRepository repo)
        {
            _repo = repo;
        }

        // Valida directamente con el método del Repository
        public async Task<bool> ValidarFuncionario(string identificacion, string contrasena)
        {
            return await _repo.ValidarFuncionario(identificacion, contrasena);
        }

        // Obtiene las áreas asociadas a un funcionario por su identificación
        public async Task<IEnumerable<Area>> ObtenerAreasPorIdentificacion(string identificacion)
        {
            var idFuncionario = await _repo.ObtenerIDFuncionario(identificacion);

            if (idFuncionario == 0)
                return Enumerable.Empty<Area>();

            return await _repo.ObtenerAreasFuncionario(idFuncionario);
        }

        // Registra una nueva marca
        public async Task<int> RegistrarMarca(string identificacion, int idArea, string detalle, string tipoMarca)
        {
            var idFuncionario = await _repo.ObtenerIDFuncionario(identificacion);

            if (idFuncionario == 0)
                throw new System.Exception("Funcionario no encontrado");

            var idMarca = await _repo.RegistrarMarca(idFuncionario, idArea, detalle, tipoMarca);

            return idMarca;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using RelojMarcador_ModuloUsuarios.Entities;

namespace RelojMarcador_ModuloUsuarios.Repository
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly string _connectionString;

        public MarcaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ProbarConexion()
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn.State.ToString();
        }

        public async Task<bool> ValidarFuncionario(string identificacion, string contrasena)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Funcionarios
                WHERE Identificacion = @identificacion
                  AND Contrasena = @contrasena;";

            using var db = new MySqlConnection(_connectionString);
            var result = await db.ExecuteScalarAsync<int>(sql, new { identificacion, contrasena });
            return result > 0;
        }


        public async Task<Funcionario?> ObtenerFuncionarioPorIdentificacion(string identificacion)
        {
            const string sql = @"SELECT ID_Funcionario, Identificacion, Contrasena, Estado 
                                 FROM Funcionarios 
                                 WHERE Identificacion = @identificacion LIMIT 1;";

            using var db = new MySqlConnection(_connectionString);
            return await db.QueryFirstOrDefaultAsync<Funcionario>(sql, new { identificacion });
        }

        public async Task<int> ObtenerIDFuncionario(string identificacion)
        {
            const string sql = @"SELECT ID_Funcionario FROM Funcionarios WHERE Identificacion = @Identificacion;";
            using var db = new MySqlConnection(_connectionString);
            var result = await db.ExecuteScalarAsync<int?>(sql, new { Identificacion = identificacion });
            return result ?? 0;
        }

        public async Task<IEnumerable<Area>> ObtenerAreasFuncionario(int idFuncionario)
        {
            const string sql = @"SELECT a.ID_Area AS Id_Area, a.Nombre_Area 
                                 FROM Funcionario_Area fa
                                 INNER JOIN Areas a ON fa.ID_Area = a.ID_Area
                                 WHERE fa.ID_Funcionario = @idFuncionario;";
            using var db = new MySqlConnection(_connectionString);
            return await db.QueryAsync<Area>(sql, new { idFuncionario });
        }

        public async Task<bool> AreaAsociadaAFuncionario(int idFuncionario, int idArea)
        {
            const string sql = @"SELECT COUNT(1) FROM Funcionario_Area 
                                 WHERE ID_Funcionario = @idFuncionario AND ID_Area = @idArea;";
            using var db = new MySqlConnection(_connectionString);
            return await db.ExecuteScalarAsync<int>(sql, new { idFuncionario, idArea }) > 0;
        }

        public async Task<int> RegistrarMarca(int idFuncionario, int idArea, string? detalle, string tipoMarca)
        {
            const string sql = @"INSERT INTO Marca (ID_Funcionario, ID_Area, Detalle, Tipo_Marca) 
                                 VALUES (@idFuncionario, @idArea, @detalle, @tipoMarca);
                                 SELECT LAST_INSERT_ID();";
            using var db = new MySqlConnection(_connectionString);
            return await db.ExecuteScalarAsync<int>(sql, new { idFuncionario, idArea, detalle, tipoMarca });
        }
    }
}

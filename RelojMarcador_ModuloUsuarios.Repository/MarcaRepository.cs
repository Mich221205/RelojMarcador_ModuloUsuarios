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

        public async Task<bool> ValidarUsuario(string identificacion, string contrasena)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM usuario
                WHERE Identificacion = @identificacion
                  AND Contrasena = @contrasena;";

            using var db = new MySqlConnection(_connectionString);
            var result = await db.ExecuteScalarAsync<int>(sql, new { identificacion, contrasena });
            return result > 0;
        }

        public async Task<int> ObtenerIDUsuario(string identificacion)
        {
            const string sql = @"SELECT ID_Usuario FROM usuario WHERE Identificacion = @Identificacion;";
            using var db = new MySqlConnection(_connectionString);
            var result = await db.ExecuteScalarAsync<int?>(sql, new { Identificacion = identificacion });
            return result ?? 0;
        }

        public async Task<IEnumerable<Area>> ObtenerAreasUsuario(int idFuncionario)
        {
            const string sql = @"SELECT a.ID_Area AS Id_Area, a.Nombre_Area 
                                 FROM usuario_area fa
                                 INNER JOIN Areas a ON fa.ID_Area = a.ID_Area
                                 WHERE fa.ID_Usuario = @idFuncionario;;";
            using var db = new MySqlConnection(_connectionString);
            return await db.QueryAsync<Area>(sql, new { idFuncionario });
        }

        public async Task<int> RegistrarMarca(int idFuncionario, int idArea, string? detalle, string tipoMarca)
        {
            const string sql = @"INSERT INTO Marca (ID_Usuario, ID_Area, Detalle, Tipo_Marca) 
                                 VALUES (@idFuncionario, @idArea, @detalle, @tipoMarca);
                                 SELECT LAST_INSERT_ID();";
            using var db = new MySqlConnection(_connectionString);
            return await db.ExecuteScalarAsync<int>(sql, new { idFuncionario, idArea, detalle, tipoMarca });
        }
    }
}

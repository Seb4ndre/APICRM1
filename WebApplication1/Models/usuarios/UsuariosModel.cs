using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Models.usuarios
{
    public class UsuariosModelMesaAyuda
    {
        private readonly string _connectionString;

        public UsuariosModelMesaAyuda()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConexionSQL"].ConnectionString;
        }

        public DataTable TestUC(string usuario, string contraseña)
        {
            var dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.TestUC", conn)) // Cambia por el SP real
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@Contraseña", contraseña);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public DataTable ObtenerBarraLateralPorRol(int rolId)
        {
            var dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.Obtener_BarraLateral_PorRol", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RolId", rolId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }



    }
}

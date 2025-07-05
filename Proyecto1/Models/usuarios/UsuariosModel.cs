using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Azure.Core;

public class UsuariosModelMesaAyuda
{
    private readonly string _connectionString;

    public UsuariosModelMesaAyuda(IConfiguration configuration)
    {
        // Lee la cadena desde appsettings.json
        _connectionString = configuration.GetConnectionString("DefaultConnection");

        // Verifica que se haya leído correctamente
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("No se pudo obtener la cadena de conexión.");
        }
    }

    public DataTable TestUC(string usuario, string contraseña)
    {
        DataTable tabla = new DataTable();

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand("dbo.TestUC", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@contraseña", contraseña);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(tabla);
        }

        return tabla;
    }

    public DataTable Ob_Permiso_User(string Tusuario)
    {
        DataTable tabla = new DataTable();

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand("ObtenerPermisosPorUsuario", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@usuario", Tusuario);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(tabla);
        }
        return (tabla);
    }
}
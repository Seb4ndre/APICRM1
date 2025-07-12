using System;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApplication1.Models.usuarios;
using WebApplication1.Models.usuarios.clases;

namespace WebApplication1.Controllers.usuarios
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("UsuariosMA")]
    public class UsuariosMAController : ApiController
    {
        private readonly UsuariosModelMesaAyuda _modelo;
        public UsuariosMAController()
        {
            _modelo = new UsuariosModelMesaAyuda(); // Ya no necesita IConfiguration
        }

        [HttpPost]
        [Route("TestUC")]
        public IHttpActionResult TestUC([FromBody] UsuariosMAClass usuariosMAClass)
        {
            if (usuariosMAClass == null || string.IsNullOrWhiteSpace(usuariosMAClass.usuario) || string.IsNullOrWhiteSpace(usuariosMAClass.contraseña))
            {
                return BadRequest("Faltan datos.");
            }

            try
            {
                DataTable resultado = _modelo.TestUC(usuariosMAClass.usuario, usuariosMAClass.contraseña);

                if (resultado.Rows.Count > 0)
                {
                    var row = resultado.Rows[0];
                    int resultadoSP = Convert.ToInt32(row["Resultado"]);
                    int? rolId = row["RolId"] != DBNull.Value ? Convert.ToInt32(row["RolId"]) : (int?)null;

                    return Ok(new
                    {
                        autenticado = resultadoSP == 1,
                        rolId = rolId
                    });
                }
                else
                {
                    return Ok(new { autenticado = false, rolId = (int?)null });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("PermisoRolUsuario")]
        public IHttpActionResult PermisoRolUsuario([FromBody] UsuariosMAClass request)
        {
            try
            {
                if (request == null || request.IdRol <= 0)
                {
                    return BadRequest("IdRol es requerido y debe ser mayor que 0.");
                }

                // Obtener los permisos para el rol enviado desde el frontend
                var resultado = _modelo.ObtenerBarraLateralPorRol(request.IdRol);

                return Ok(new
                {
                    rolId = request.IdRol,
                    barra = resultado
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("TraerUsuarios")]
        public IHttpActionResult TraerUsuarios()
        {
            try
            {
                var resultado = _modelo.TraerUsuarios();

                // Convertir DataTable a lista de objetos anónimos
                var lista = resultado.AsEnumerable().Select(row => new
                {
                    NombreCompleto = row["NombreCompleto"].ToString(),
                    Correo = row["Correo"].ToString(),
                    Empresa = row["Empresa"].ToString(),
                    Rol = row["Rol"].ToString()
                }).ToList();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("TraerPorUsuario")]
        public IHttpActionResult TraerPorUsuario([FromBody] UsuarioRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username))
                return BadRequest("Username es requerido.");

            try
            {
                var tabla = _modelo.TraerPHSporUsuario(request.Username);

                var lista = tabla.AsEnumerable().Select(row => new
                {
                    PHSId = Convert.ToInt32(row["PHSId"]),
                    Nombre = row["Nombre"].ToString(),
                    Direccion = row["Direccion"].ToString()
                }).ToList();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}

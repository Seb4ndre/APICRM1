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
            // Validar entrada
            if (usuariosMAClass == null || string.IsNullOrWhiteSpace(usuariosMAClass.usuario) || string.IsNullOrWhiteSpace(usuariosMAClass.contraseña))
            {
                return BadRequest("Faltan datos.");
        }

            // Lógica de autenticación
            bool autenticado = _modelo.TestUC(usuariosMAClass.usuario, usuariosMAClass.contraseña)
                                .AsEnumerable()
                                .Any(row => Convert.ToInt32(row["Resultado"]) == 1);

            // Devolver directamente true o false
            return Ok(autenticado);
        }
        [HttpPost]
        [Route("Ob_Permiso_User")]
        public IHttpActionResult Ob_Permiso_User([FromBody] UsuariosMAClass request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Tusuario))
                {
                    return BadRequest("Username es requerido.");
                }

                var resultado = _modelo.Ob_Permiso_User(request.Tusuario);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}

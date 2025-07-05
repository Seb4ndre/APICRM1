using System;
using System.Data;
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
            var resultado = _modelo.TestUC(usuariosMAClass.usuario, usuariosMAClass.contraseña);
            return Ok(new { autenticado = true });
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

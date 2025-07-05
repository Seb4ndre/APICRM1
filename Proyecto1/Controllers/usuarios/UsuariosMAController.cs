using System.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Proyecto1.Models.usuarios;
using Proyecto1.Models.usuarios.clases;

namespace Proyecto1.Controllers.usuarios
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosMAController : ControllerBase
    {
        private readonly UsuariosModelMesaAyuda _modelo;

        public UsuariosMAController(IConfiguration configuration)
        {
            _modelo = new UsuariosModelMesaAyuda(configuration);
        }

        [HttpPost]
        [Route("TestUC")]
        public IActionResult TestUC([FromBody] UsuariosMAClass usuariosMAClass)
        {
            if (usuariosMAClass == null || string.IsNullOrEmpty(usuariosMAClass.usuario) || string.IsNullOrEmpty(usuariosMAClass.contraseña))
            {
                return BadRequest("Usuario o contraseña no proporcionados.");
            }

            var resultado = _modelo.TestUC(usuariosMAClass.usuario, usuariosMAClass.contraseña);

            if (resultado.Rows.Count > 0 && Convert.ToInt32(resultado.Rows[0]["Resultado"]) == 1)
            {
                return Ok(new { autenticado = true });
            }
            else
            {
                return Unauthorized(new { autenticado = false });
            }
        }
        [HttpPost("Ob_Permiso_User")]
        public IActionResult Ob_Permiso_User([FromBody] UsuariosMAClass request)
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
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }

}

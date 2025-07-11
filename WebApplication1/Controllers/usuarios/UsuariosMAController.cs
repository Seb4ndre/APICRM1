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

        //LOGIN
        [HttpPost]
        [Route("TestUC")]
        public IHttpActionResult TestUC([FromBody] UsuariosMAClass usuariosMAClass)
        {
            if (usuariosMAClass == null || string.IsNullOrWhiteSpace(usuariosMAClass.usuario) || string.IsNullOrWhiteSpace(usuariosMAClass.contraseña))
            {
                return BadRequest("Faltan datos.");
            }

            bool autenticado = _modelo.TestUC(usuariosMAClass.usuario, usuariosMAClass.contraseña)
                                .AsEnumerable()
                                .Any(row => Convert.ToInt32(row["Resultado"]) == 1);

            return Ok(autenticado);
        }

        //PERMISOS
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

        //REGISTRAR USUARIO
        [HttpPost]
        [Route("RegistrarUsuario")]
        public IHttpActionResult RegistrarUsuario([FromBody] UsuariosMAClass nuevoUsuario)
        {
            if (nuevoUsuario == null || string.IsNullOrWhiteSpace(nuevoUsuario.usuario))
                return BadRequest("Datos incompletos.");

            try
            {
                bool insertado = _modelo.RegistrarUsuario(nuevoUsuario);
                return insertado ? Ok("Usuario registrado.") : BadRequest("No se pudo registrar.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //LISTAR USUARIOS
        [HttpGet]
        [Route("ListarUsuarios")]
        public IHttpActionResult ListarUsuarios()
        {
            try
            {
                var usuarios = _modelo.ObtenerTodos();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //OBTENER USUARIO POR ID
        [HttpGet]
        [Route("ObtenerUsuario/{id}")]
        public IHttpActionResult ObtenerUsuario(int id)
        {
            try
            {
                var usuario = _modelo.ObtenerPorId(id);
                if (usuario == null)
                    return NotFound();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //ELIMINAR USUARIO
        [HttpDelete]
        [Route("EliminarUsuario/{id}")]
        public IHttpActionResult EliminarUsuario(int id)
        {
            try
            {
                bool eliminado = _modelo.Eliminar(id);
                return eliminado ? Ok("Usuario eliminado.") : BadRequest("No se pudo eliminar.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //ACTUALIZAR USUARIO
        [HttpPut]
        [Route("ActualizarUsuario/{id}")]
        public IHttpActionResult ActualizarUsuario(int id, [FromBody] UsuariosMAClass usuarioActualizado)
        {
            if (usuarioActualizado == null)
                return BadRequest("Datos inválidos.");

            try
            {
                bool actualizado = _modelo.Actualizar(id, usuarioActualizado);
                return actualizado ? Ok("Usuario actualizado.") : BadRequest("No se pudo actualizar.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}


using System.Threading.Tasks;
using ApiJwt.Models;
using ApiJwt.Repository;
using ApiJwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiJwt
{
    [Route("v1/conta")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        //dynamic porque haverão varios tipos de retorno
        public async Task<ActionResult<dynamic>> Autenticar([FromServices] User modelUser)
        {
            var usuario = UserRepository.GetLogin(modelUser.Username, modelUser.Password);

            if (usuario == null)
                return NotFound(new { message = "usuario ou senha invalidos" });

            var token = TokenServices.GerarToken(usuario);
            usuario.Password = "";
            return new
            {
                usuario = usuario,
                token = token
            };
        }

        [HttpGet]
        [Route("Anonimo")]
        [AllowAnonymous]
        public string Anonimo() => "Anonimous";


        //Apartir daqui Somente serão autorizados apos gerar o token
        [HttpGet]
        [Route("autenticate")]
        [Authorize]
        public string Autenticate() => string.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("funcionario")]
        [Authorize(Roles = "employee,manager")]
        public string Funcionario()
        {
            return string.Format("Funcionario {0} Autenticado ", User.Identity.Name);
        }

        [HttpGet]
        [Route("gerente")]
        [Authorize(Roles = "manager")]
        public string Gerente()
        {
            return string.Format("Gerente {0} Autenticado ", User.Identity.Name);
        }
    }
}
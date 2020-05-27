using System.Threading.Tasks;
using ApiJwt.Models;
using ApiJwt.Repository;
using ApiJwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiJwt
{
    [Route("v1/signIn")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        //dynamic porque haverão varios tipos de retorno
        public async Task<ActionResult<dynamic>> Autenticar([FromBody]User model)
        {
            // Recupera o usuário
            var user = UserRepository.GetLogin(model.Username, model.Password);
            //var user = new  Models.User {Id = 1, Username ="robin", Password="robin", Role="employee" };

            // Verifica se o usuário existe
            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            var token = TokenServices.GerarToken(user);

            // Oculta a senha
            user.Password = "";

            // Retorna os dados
            return new
            {
                user = user,
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
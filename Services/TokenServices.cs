using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiJwt.Models;
using Microsoft.IdentityModel.Tokens;


namespace ApiJwt.Services
{
    public static class TokenServices
    {
        public static string GerarToken(User user)
        {
            //JwtSecurityTokenHandler - metodo que gera o token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Econdando (transformando para bytes)
            var key = Encoding.ASCII.GetBytes(Settings.SecretKeyPrivate);

            //Descrição do Token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Subject - Clains que ficam disponivel - por enquanto serão: name, role
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),

                //Expires - quando o token expira
                Expires = DateTime.UtcNow.AddHours(1),

                //SigningCredentials - parametros: SymmetricSecurityKey é a chave que será passada,  e o HmacSha256Signature é o algoritmos hash para encriptação
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //CreateToken - Metodo que cria o token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //WriteToke : leitura de um token e conversão para string
            return tokenHandler.WriteToken(token);
        }
    }
}

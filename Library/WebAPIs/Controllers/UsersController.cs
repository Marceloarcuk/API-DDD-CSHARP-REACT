using Entities.Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebApi.Token;
using WebAPIs.Models;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UsersController(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [AllowAnonymous]     // QUALQUER USUARIO PODE ACESSAR
        [Produces("application/json")]
        [HttpPost("/api/CriarToken")]
        public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) ||
                string.IsNullOrWhiteSpace(login.Senha))
            {
                return Unauthorized();
            }
            var resultado = await _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                var userCurrent = await _userManager.FindByEmailAsync(login.Email); 
                var idUser = userCurrent.Id;
                var token = new TokenJWTBuilder()
                                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                                    .AddSubject("MAC-DEV")
                                    .AddIssuer("MAC.Security.Bearer")
                                    .AddAudience("MAC.Security.Bearer")
                                    .AddClaim("idUser", idUser)
                                    .AddExpiry(5)
                                    .Builder();
                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]     // QUALQUER USUARIO PODE ACESSAR
        [Produces("application/json")]
        [HttpPost("/api/AdicionarUsuario")]
        public async Task<IActionResult> AdicionarUsuario([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) ||
                string.IsNullOrWhiteSpace(login.Senha))
            {
                return Ok("Falta alguns dados.");
            }

            var user = new ApplicationUser
            {
                Email = login.Email,
                UserName = login.Email,
                CPF = login.CPF,
                Tipo = TipoUsuario.Comum
            };

            var result = await _userManager.CreateAsync(user, login.Senha);
            if (result.Errors.Any())
            {
                return Ok(result.Errors);
            }


            // GERACAO DE CONFIRMACAO POR EMAIL - precisa implementar
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // RETORNO DO EMAIL DE CONFIRMACAO
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var response_Retorno = await _userManager.ConfirmEmailAsync(user, code);
            if (response_Retorno.Succeeded)
            {
                return Ok("Usuario adicionado");
            }
            else
            {
                return Ok("Erro ao confirmar usuario.");
            }
        }



    }

}

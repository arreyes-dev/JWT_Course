using JWTApiMinimal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JWTApiMinimal.Controllers
{
    [ApiController]
    [Route("UserController")]
    public class UserController : Controller
    {

        public IConfiguration _configuration;

        public UserController(IConfiguration configuration) { 
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public dynamic Login([FromBody] Object optData) {
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString()!);
            string user = data!.user.ToString();
            string password = data!.password.ToString();

            var userLogin = UserModel.GetUserDB().FirstOrDefault(x => x.userName == user && x.userPwd == password);

            if (userLogin == null)
            {
                return new
                {
                    success = true,
                    message = "Error",
                    result = userLogin,
                };
            }
            // 1. Se obtiene la configuración JWT desde el archivo appsettings.json,
            //    que incluye la clave secreta, el issuer, el audience, y el subject.
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>(); // Obtenemos la configuración de ./appsettings.json

            // 2. Crear cada claim de forma individual y explicada:
            // Claim estándar: "sub" (subject)
            // Indica quién es el dueño del token, generalmente representa al usuario autenticado.
            var subClaim = new Claim(JwtRegisteredClaimNames.Sub, jwt!.Subject!);
            // Claim estándar: "jti" (JWT ID)
            // Un identificador único para este token, útil para evitar ataques de repetición (replay).
            var jtiClaim = new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            // Claim estándar: "iat" (issued at)
            // Fecha y hora de emisión del token (formato UTC). Ayuda a validar cuánto tiempo ha pasado desde su creación.
            var iatClaim = new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString());
            // Claim personalizado: "id"
            // El ID del usuario autenticado, útil para buscarlo luego en la base de datos.
            var idClaim = new Claim("id", userLogin.Id.ToString());
            // Claim personalizado: "user"
            // El nombre de usuario autenticado, puede servir para mostrar en la UI o para logs.
            var userClaim = new Claim("user", userLogin.userName!);
            // Agrupar todos los claims en un arreglo
            var claims = new[] { subClaim, jtiClaim, iatClaim, idClaim, userClaim };


            // 3. Se crea la clave de firma usando la clave secreta desde la configuración.
            //    Esta clave debe tener al menos 256 bits (32 caracteres).
            var keyS = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwt.Key!));

            // 4. Se definen las credenciales de firma utilizando HMAC SHA256 con la clave generada.
            var singIn = new SigningCredentials(keyS, SecurityAlgorithms.HmacSha256);

            // 5. Finalmente, se construye el token JWT con:
            //    - el issuer (quién emite el token),
            //    - el audience (quién debe aceptar el token),
            //    - los claims (información del usuario),
            //    - la fecha de expiración (60 minutos desde ahora),
            //    - las credenciales de firma (para validar autenticidad e integridad).
            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: singIn
            );


            return new
                {
                    success = true,
                    message = "Ok",
                    result = new JwtSecurityTokenHandler().WriteToken(token),
                };
        }
    }
}

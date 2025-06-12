using System.Security.Claims;

namespace JWTApiMinimal.Models
{
    public class Jwt
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; } 
        public string? Audience { get; set; } 
        public string? Subject { get; set; }     
        public static dynamic validToken(ClaimsIdentity identity) {
            try
            {
                if (identity.Claims.Count() == 0) { 
                    return new
                        {
                            success = false,
                            message = "Token no encontrado",
                            result = ""
                        };
                }

                string idUser = identity.Claims.FirstOrDefault(x => x.Type == "id")!.Value;
                UserModel userF = UserModel.GetUserDB().FirstOrDefault(x => x.Id.ToString() == idUser);
                return new
                {
                    success = true,
                    message = "OK",
                    result = userF
                };
            }
            catch ( Exception ex ) {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.ToString(),
                    result = ""
                };
             }
        
        }
    }

    

}

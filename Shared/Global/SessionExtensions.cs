using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shared.Global
{
    public static class SessionExtensions
    {
        public static async Task ObtenerDatosTokenAsync(this StringValues headerValues, InformationSession informationSession)
        {
            string token = headerValues.FirstOrDefault()!;
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Replace("Bearer ", "");

                var handler = new JwtSecurityTokenHandler();
                var tokenDecode = handler.ReadJwtToken(token);
                string roleName = string.Empty;
                foreach (var itm in tokenDecode.Claims.Where(x => x.Type.Equals(ClaimTypes.Role)))
                {
                    if (roleName.IsNullOrEmpty())
                        roleName = itm.Value;
                    informationSession.Role = itm.Value.ConvertObjectToInt();
                }

                informationSession.RoleName = roleName.ReplaceIfNullOrEmpty("none");
                informationSession.FirstName = (tokenDecode.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Name))?.Value).ReplaceIfNullOrEmpty("[ANONIMO]");
                informationSession.UserName = (tokenDecode.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.NameId))?.Value).ReplaceIfNullOrEmpty();
                informationSession.IdCard = (tokenDecode.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sid))?.Value).ReplaceIfNullOrEmpty();
                informationSession.UserId = (tokenDecode.Claims.FirstOrDefault(x => x.Type.Equals("uid"))?.Value).ReplaceIfNullOrEmpty("0").ConvertObjectToInt();
            }
            await Task.CompletedTask;
        }

    }

}

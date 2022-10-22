using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace MiddleWare
{
    public class DetectContextMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ContextRequest _contextRequest;

        public DetectContextMiddleWare(RequestDelegate next, ContextRequest contextRequest)
        {
            _next = next;
            _contextRequest = contextRequest;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            StringValues tokenObject = "";
            if (context.Request.Headers.TryGetValue("Authorization", out tokenObject))
            {
                String accessToken = tokenObject.ToString().Replace("bearer ", "");
                accessToken = accessToken.ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var accessTokenHandled = handler.ReadJwtToken(accessToken);

                var emailCurrentUser = accessTokenHandled.Payload.GetValueOrDefault("userEmail");
                if (emailCurrentUser != null)
                {
                    _contextRequest.SetEmailCurrentUser(emailCurrentUser.ToString());
                }
            }
            await _next(context);
        }
    }
}

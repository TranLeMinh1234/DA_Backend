using ClassModel.TaskRelate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace MiddleWare
{
    public class WebSocketMiddleWare
    {
        private readonly RequestDelegate _next;

        private readonly WebsocketConnectionManager _manager;

        private readonly ContextRequest _contextRequest;

        public WebSocketMiddleWare(RequestDelegate next, WebsocketConnectionManager manager, ContextRequest contextRequest)
        {
            _next = next;
            _manager = manager;
            _contextRequest = contextRequest;
        }

        public async System.Threading.Tasks.Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("connect success");

                string emailDetect = await DetectTokenInfo(context);

                if (String.IsNullOrEmpty(emailDetect))
                {
                    return;
                }

                _manager.AddSocket(webSocket, emailDetect);

                await Receive(webSocket, async (result, buffer) => {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        Console.WriteLine($"Receive->Text");
                        Console.WriteLine($"Message: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        return;
                    }
                });
            }
            else
            {
                await _next(context);
            }
        }

        public async System.Threading.Tasks.Task<string> DetectTokenInfo(HttpContext context)
        {
            StringValues tokenObject = "";
            
            if (context.Request.Query.TryGetValue("token",out tokenObject))
            {
                if (String.IsNullOrEmpty(tokenObject.ToString()))
                {
                    return null;
                }

                String accessToken = tokenObject.ToString();
                var handler = new JwtSecurityTokenHandler();
                var accessTokenHandled = handler.ReadJwtToken(accessToken);

                var emailCurrentUser = accessTokenHandled.Payload.GetValueOrDefault("userEmail");
                if (emailCurrentUser != null)
                {
                    _contextRequest.SetEmailCurrentUser(emailCurrentUser.ToString());
                }

                return emailCurrentUser.ToString();
            }

            return null;
        }

        private async System.Threading.Tasks.Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 6];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}

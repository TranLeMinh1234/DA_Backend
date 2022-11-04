using ClassModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WebsocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocketCustom> _sockets = new ConcurrentDictionary<string, WebSocketCustom>();

        public string AddSocket(WebSocket newSocket, string email)
        {
            WebSocketCustom newWebSocketCustom = new WebSocketCustom()
            {
                WebSocketInheritance = newSocket
            };
            _sockets.AddOrUpdate(email, newWebSocketCustom, (key,oldWebSocketCustom) => { 
                return newWebSocketCustom;
            });

            return email;
        }

        public ConcurrentDictionary<string, WebSocketCustom> GetAllSocket()
        {
            return _sockets;
        }

        public async Task SendMessageToUser(string email, string text) {
            WebSocketCustom webSocketCustom = null;
            if (_sockets.TryGetValue(email, out webSocketCustom))
            { 
                await webSocketCustom.SendText(text);
            }
        }
    }
}

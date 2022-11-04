using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassModel
{
    public class WebSocketCustom
    {
        public WebSocket WebSocketInheritance { get; set; }

        public async Task SendText(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            await WebSocketInheritance.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}

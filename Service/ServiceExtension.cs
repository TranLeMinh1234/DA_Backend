using Microsoft.Extensions.DependencyInjection;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public static class ServiceExtension
    {
        public static IServiceCollection UseContextRequestService(this IServiceCollection services)
        {
            return services.AddSingleton<ContextRequest>();
        }

        public static IServiceCollection UseWebSocketConnectionManagerService(this IServiceCollection services)
        {
            return services.AddSingleton<WebsocketConnectionManager>();
        }

        public static IServiceCollection UseRemindTaskService(this IServiceCollection services)
        {
            return services.AddSingleton<RemindTaskService>();
        }
    }
}

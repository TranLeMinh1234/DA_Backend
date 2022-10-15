using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleWare
{
    public static class MiddleWareExtension
    {
        public static IApplicationBuilder UseDetectContextMiddleWare(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<DetectContextMiddleWare>();
        }
    }
}

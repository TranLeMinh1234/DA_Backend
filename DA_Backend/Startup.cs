using BL;
using BL.Business;
using BL.FileAttachment;
using BL.Interface;
using DL;
using DL.Business;
using DL.DLFile;
using DL.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MiddleWare;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Backend
{
    public class Startup
    {
        #region property
        public IConfiguration Configuration { get; }

        private string CORSConfig = "CORSConfig";

        #endregion

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IConfiguration>(Configuration);

            //dj
            //DL
            services.AddScoped<IDLBase,DLBase>();
            services.AddScoped<IDLFileAttachment, DLFileAttachment>();
            services.AddScoped<IDLLogin, DLLogin>();
            services.AddScoped<IDLLabel, DLLabel>();
            services.AddScoped<IDLTask, DLTask>();
            services.AddScoped<IDLUser, DLUser>();
            services.AddScoped<IDLCheckTask, DLCheckTask>();
            services.AddScoped<IDLComment, DLComment>();

            //BL
            services.AddScoped<IBLBase, BLBase>();
            services.AddScoped<IBLFileAttachment, BLFileAttachment>();
            services.AddScoped<IBLLogin, BLLogin>();
            services.AddScoped<IBLLabel, BLLabel>();
            services.AddScoped<IBLTask, BLTask>();
            services.AddScoped<IBLUser, BLUser>();
            services.AddScoped<IBLCheckTask, BLCheckTask>();
            services.AddScoped<IBLComment, BLComment>();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddCors(options => {
                options.AddPolicy(name: CORSConfig,
                        builder => {
                            builder.AllowAnyHeader()
                                   .AllowAnyMethod()
                                   .AllowAnyOrigin();
                        }
                    );
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:KeyLoggin"]))
                    };
                }
            );

            services.UseContextRequestService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CORSConfig);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseDetectContextMiddleWare();

            //app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

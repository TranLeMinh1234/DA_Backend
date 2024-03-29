using BL;
using BL.Business;
using BL.FileAttachment;
using BL.Interface;
using ClassModel.Email;
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
using Quartz;
using Service;
using Service.Interface;
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
            services.AddScoped<IDLTemplateGroupTask, DLTemplateGroupTask>();
            services.AddScoped<IDLRole, DLRole>();
            services.AddScoped<IDLGroupTask, DLGroupTask>();
            services.AddScoped<IDLNotification, DLNotification>();
            services.AddScoped<IDLTemplateCustom, DLTemplateCustom>();

            //BL
            services.AddScoped<IBLBase, BLBase>();
            services.AddScoped<IBLFileAttachment, BLFileAttachment>();
            services.AddScoped<IBLLogin, BLLogin>();
            services.AddScoped<IBLLabel, BLLabel>();
            services.AddScoped<IBLTask, BLTask>();
            services.AddScoped<IBLUser, BLUser>();
            services.AddScoped<IBLCheckTask, BLCheckTask>();
            services.AddScoped<IBLComment, BLComment>();
            services.AddScoped<IBLTemplateGroupTask, BLTemplateGroupTask>();
            services.AddScoped<IBLRole, BLRole>();
            services.AddScoped<IBLGroupTask, BLGroupTask>();
            services.AddScoped<IBLNotification, BLNotification>();
            services.AddScoped<IBLTemplateCustom, BLTemplateCustom>();

            //scoped servies
            services.AddSingleton<RemindTaskJob>();

            services.AddSingleton<IConfiguration>(Configuration);

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddSingleton<MailService>();

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

            //service
            services.UseContextRequestService();
            services.UseWebSocketConnectionManagerService();
            services.UseRemindTaskService();
            services.UseRemindTaskJob();
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

            app.UseWebSockets();

            app.UseWebSocketMiddleWare();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

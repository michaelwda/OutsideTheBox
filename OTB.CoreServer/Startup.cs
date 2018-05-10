using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OTB.CoreServer.ConnectionHandlers;

namespace OTB.CoreServer{
	public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConnections();
            services.AddSignalR(option => { option.KeepAliveInterval = TimeSpan.FromSeconds(5); }).AddMessagePackProtocol();
             

        

            services.AddCors(o =>
            {
                o.AddPolicy("Everything", p =>
                {
                    p.AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowAnyOrigin()
                     .AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFileServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("Everything");
             
            app.UseSignalR(routes =>
            {
				routes.MapHub<OTBHub>("/OTB");
             
            });
            app.UseConnections(routes =>
            {
                routes.MapConnectionHandler<MessagesConnectionHandler>("/OTB");
            });

        }

        
    }
}
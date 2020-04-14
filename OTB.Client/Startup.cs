using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OTB.CoreServer{
	public class Startup
    {
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
                        .AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseFileServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseCors("Everything");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<OTBHub>("/OTB");

                //endpoints.MapConnectionHandler<MessagesConnectionHandler>("/OTB");
            });
          
        }

        
    }
}
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OTB.Client 
{
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
            });
        }
    }
}
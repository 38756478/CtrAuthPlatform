using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ctr.AhphOcelot.Middleware;
using IdentityServer4.AccessTokenValidation;
using Ocelot.Administration;

namespace Ctr.AuthPlatform.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Action<IdentityServerAuthenticationOptions> options = o =>
            {
                o.Authority = "http://localhost:6611"; //IdentityServer��ַ
                o.RequireHttpsMetadata = false;
                o.ApiName = "gateway_admin"; //���ع�������ƣ���Ӧ��Ϊ�ͻ�����Ȩ��scope
            };
            services.AddOcelot().AddAhphOcelot(option =>
            {
                option.DbConnectionStrings = "Server=localhost;Database=Ctr_AuthPlatform;User ID=root;Password=bl123456;";
                //option.EnableTimer = true;//���ö�ʱ����
                //option.TimerDelay = 10 * 000;//����10��
            })
            .UseMySql()
            .AddAdministration("/CtrOcelot", options);
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseAhphOcelot().Wait();
        }
    }
}

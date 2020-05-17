using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DBProxy.Application.Queries;
using DBProxy.Infra.AutoFac;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DBProxy
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        //START Older way to register Module using Autofac (.Net core 2.2)
        // public IContainer AppContainer { get; private set; }
        //END Older way to register Module using Autofac (.Net core 2.2)

        ////START New way to register Module using Autofac (.Net core 3.1)
        public ILifetimeScope AutofacContainer { get; private set; }
        ////END New way to register Module using Autofac (.Net core 3.1)

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddSingleton<IQuery,Query>();
            services.AddControllers();
            services.AddCors(option =>
            {
                option.AddPolicy("CORS", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });

            services.AddOptions();

            //START Older way to register Module using Autofac (.Net core 2.2)

            //var builder = new ContainerBuilder();
            //builder.RegisterModule(new DBProxyAPIModule(Configuration));
            //builder.Populate(services);
            //AppContainer = builder.Build();
            // return new AutofacServiceProvider(AppContainer);

            //END Older way to register Module using Autofac (.Net core 2.2)
        }

        ////START New way to register Module using Autofac (.Net core 3.1)
        public void ConfigureContainer(ContainerBuilder builder)
        {            
            builder.RegisterModule(new DBProxyAPIModule(Configuration));
        }
        ////END New way to register Module using Autofac (.Net core 3.1)

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ////START New way to register Module using Autofac (.Net core 3.1)
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            ////END New way to register Module using Autofac (.Net core 3.1)
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

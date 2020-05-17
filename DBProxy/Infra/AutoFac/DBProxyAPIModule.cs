using Autofac;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Caching.Memory;
using System.Data.SqlClient;
using System.Data;
using DBProxy.Enumrations;
using Microsoft.Extensions.Configuration;
using DBProxy.DBInfraRepository;
using DBProxy.DomainModel;

namespace DBProxy.Infra.AutoFac
{
    public class DBProxyAPIModule : Module
    {
        private readonly IConfiguration _configuration;

        public DBProxyAPIModule(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        protected override void Load(ContainerBuilder containerBuilder)
        {
            base.Load(containerBuilder);

            containerBuilder.RegisterGeneric(typeof(Logger<>)).As(typeof(Logger<>));
            containerBuilder.RegisterType<MemoryCache>().As<MemoryCache>();
            containerBuilder.RegisterType<Repository<History>>().As<IRepository<History>>();

            containerBuilder.Register(c =>
            {
                return new SqlConnection(_configuration.GetSection("connectionstrings:connectionstringread").Value);
            }).As<IDbConnection>()
            .ExternallyOwned()
            .InstancePerDependency().Keyed<IDbConnection>(SqlConnectionType.connectionstringRead);

            containerBuilder.Register(c =>
            {
                return new SqlConnection(_configuration.GetSection("connectionstrings:connectionstringwrite").Value);
            }).As<IDbConnection>()
            .ExternallyOwned()
            .InstancePerDependency().Keyed<IDbConnection>(SqlConnectionType.connectionstringWrite);

            containerBuilder.Register(c =>
            {
                return new SqlConnection(_configuration.GetSection("connectionstrings:connectionstringreport").Value);
            }).As<IDbConnection>()
            .ExternallyOwned()
            .InstancePerDependency().Keyed<IDbConnection>(SqlConnectionType.connectionstringReport);

            containerBuilder.Register<Func<SqlConnectionType, IDbConnection>>(c =>
            {
                var componentContext = c.Resolve<IComponentContext>();
                return (name) =>
                {
                    var dataService = componentContext.ResolveKeyed<IDbConnection>(name);
                    return dataService;
                };
            });
        }
    }
}

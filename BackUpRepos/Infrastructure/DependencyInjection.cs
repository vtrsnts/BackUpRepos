using BackUpRepos.Infrastructure.ExternalService.AzureDevOps;
using BackUpRepos.Infrastructure.Service.LocalFile;
using BackUpRepos.Infrastructure.Service.LocalGit;
using BackUpRepos.Model.Config;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BackUpRepos.Infrastructure
{
    [ExcludeFromCodeCoverage]
    internal static class DependencyInjection
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddConfig(services, configuration);
            AddServices(services, configuration);
        }

        private static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ILocalGitService, LocalGitService>();
            services.AddSingleton<IAzureDevOpsGit, AzureDevOpsGit>();
            services.AddSingleton<IRepoFileStorageService, RepoFileStorageService>();


            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());              
            });

        }

        private static void AddConfig(IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection azureDevOpsConfig = configuration.GetSection("AzureDevOpsConfig");
            services.Configure<AzureDevOpsConfig>(azureDevOpsConfig);
            IConfigurationSection repoFileStorageConfig = configuration.GetSection("RepoFileStorageConfig");
            services.Configure<RepoFileStorageConfig>(repoFileStorageConfig);
        }
    }
}

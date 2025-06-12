using BackUpRepos.Features.AzureDevOpsGits;
using BackUpRepos.Features.BackupRepos;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackUpRepos;

internal class Program
{

    static async Task Main(string[] args)
    {

        var services = ConfigureServices();
        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var getAzureDevOpsGitResponse = await mediator.Send(new GetAzureDevOpsGitRequest());
        await mediator.Send(new BackupRepoRequest { Repos = getAzureDevOpsGitResponse.Repos });
    }
    private static IServiceCollection ConfigureServices()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        serviceCollection.AddSingleton(configuration);
        Infrastructure.DependencyInjection.ConfigureServices(serviceCollection, configuration);
        return serviceCollection;
    }
}

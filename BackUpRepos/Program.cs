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
        ConfigureLayOut();
        var services = ConfigureServices();
        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var getAzureDevOpsGitResponse = await mediator.Send(new GetAzureDevOpsGitCommand());
        await mediator.Send(new BackupRepoCommand { Repos = getAzureDevOpsGitResponse.Repos });
        Console.WriteLine("Backup completed successfully.");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
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
    private static void ConfigureLayOut() 
    {
        Console.ForegroundColor = ConsoleColor.Green;
    }
}

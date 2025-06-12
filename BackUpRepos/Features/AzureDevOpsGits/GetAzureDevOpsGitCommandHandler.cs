using BackUpRepos.Infrastructure.ExternalService.AzureDevOps;
using MediatR;

namespace BackUpRepos.Features.AzureDevOpsGits;

public class GetAzureDevOpsGitCommandHandler : IRequestHandler<GetAzureDevOpsGitCommand, GetAzureDevOpsGitCommandResult>
{
    private readonly IAzureDevOpsGit _azureDevOpsGit;
    public GetAzureDevOpsGitCommandHandler(IAzureDevOpsGit azureDevOpsGit)
    {
        _azureDevOpsGit = azureDevOpsGit;
    }
    public async Task<GetAzureDevOpsGitCommandResult> Handle(GetAzureDevOpsGitCommand request, CancellationToken cancellationToken)
    {
       var  response  =  await _azureDevOpsGit.GetRepos();
        return new GetAzureDevOpsGitCommandResult
        {
            Repos = response
        };
    }
}



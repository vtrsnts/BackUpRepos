using BackUpRepos.Infrastructure.ExternalService.AzureDevOps;
using MediatR;

namespace BackUpRepos.Features.AzureDevOpsGits;

public class GetAzureDevOpsGitHandler : IRequestHandler<GetAzureDevOpsGitRequest, GetAzureDevOpsGitResponse>
{
    private readonly IAzureDevOpsGit azureDevOpsGit;
    public GetAzureDevOpsGitHandler(IAzureDevOpsGit azureDevOpsGit)
    {
        this.azureDevOpsGit = azureDevOpsGit;
    }
    public async Task<GetAzureDevOpsGitResponse> Handle(GetAzureDevOpsGitRequest request, CancellationToken cancellationToken)
    {
       var  response  =  await azureDevOpsGit.GetRepos();
        return new GetAzureDevOpsGitResponse
        {
            Repos = response
        };
    }
}



using BackUpRepos.Model;

namespace BackUpRepos.Features.AzureDevOpsGits;

public class GetAzureDevOpsGitCommandResult 
{
    public List<GetReposResponse> Repos { get; set; } = new List<GetReposResponse>();
}

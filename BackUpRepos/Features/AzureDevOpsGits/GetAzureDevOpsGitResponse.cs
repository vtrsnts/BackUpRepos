using BackUpRepos.Model;
using MediatR;

namespace BackUpRepos.Features.AzureDevOpsGits;

public class GetAzureDevOpsGitResponse 
{
    public List<GetReposResponse> Repos { get; set; } = new List<GetReposResponse>();
}

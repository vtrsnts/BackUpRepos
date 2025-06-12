using BackUpRepos.Model;

namespace BackUpRepos.Infrastructure.ExternalService.AzureDevOps
{
    public interface IAzureDevOpsGit
    {
        Task<List<GetReposResponse>> GetRepos();
    }
}
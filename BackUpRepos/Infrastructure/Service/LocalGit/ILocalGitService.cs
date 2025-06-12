using BackUpRepos.Model;

namespace BackUpRepos.Infrastructure.Service.LocalGit;

public interface ILocalGitService
{
    Task AddOrUpdateAsync(List<GetReposResponse> repoResponses);
}
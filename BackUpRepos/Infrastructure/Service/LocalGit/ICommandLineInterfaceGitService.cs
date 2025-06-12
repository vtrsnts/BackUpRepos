using BackUpRepos.Model;

namespace BackUpRepos.Infrastructure.Service.LocalGit;

public interface ICommandLineInterfaceGitService
{
    Task AddOrUpdateAsync(List<GetReposResponse> repoResponses);
}
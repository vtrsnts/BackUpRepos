using BackUpRepos.Model;

namespace BackUpRepos.Infrastructure.Service.LocalFile
{
    public interface IRepoFileStorageService
    {
        Repo? GetRepo(string repoName, string branch);
        IList<Repo> GetSuccessRepos();
        void SaveSuccessRepos(Repo repo);       
    }
}
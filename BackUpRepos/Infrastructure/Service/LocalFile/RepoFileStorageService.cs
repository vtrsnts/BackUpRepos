using BackUpRepos.Model;
using BackUpRepos.Model.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BackUpRepos.Infrastructure.Service.LocalFile;

public class RepoFileStorageService : IRepoFileStorageService
{
    private readonly RepoFileStorageConfig _repoFileStorageConfig;
    private readonly string _fileName = $@"{DateTime.Now.ToString("dd-MM-yyyy")}.json";
    private readonly string _fullPathFile;
    private IList<Repo> _repos;
    public RepoFileStorageService(IOptions<RepoFileStorageConfig> repoFileStorageConfig)
    {
        _repoFileStorageConfig = repoFileStorageConfig.Value;
        _fullPathFile = $@"{_repoFileStorageConfig.StoragePath}\{_fileName}";
    }
    public IList<Repo> GetSuccessRepos()
    {
        _repos = new List<Repo>();

        if (File.Exists(_fullPathFile))
            _repos = JsonConvert.DeserializeObject<List<Repo>>(File.ReadAllText(_fullPathFile));
        return _repos;
    }
    public void SaveSuccessRepos(Repo repo)
    {
        GetSuccessRepos();
        _repos.Add(repo);
        string json = JsonConvert.SerializeObject(_repos, Formatting.Indented);
        File.WriteAllText(_fullPathFile, json);
    }
    public Repo? GetRepo(string repoName, string branch)
    {
        Repo? repo = null;
        if (_repos != null)
            repo = _repos.FirstOrDefault(r => r.RepoName == repoName && r.Branch == branch);
        return repo;
    }
}

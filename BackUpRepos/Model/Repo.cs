namespace BackUpRepos.Model;

public class Repo
{
    public Repo(string repoName, string branch)
    {
        RepoName = repoName;
        Branch = branch;
    }

    public string RepoName { get; set; }
    public string Branch { get; set; }

}

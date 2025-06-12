namespace BackUpRepos.Model;

public record GetReposResponse(string Name, string RemoteUrl, List<string> BranchList)
{

}
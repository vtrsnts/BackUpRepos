namespace BackUpRepos.Model.Config;

public class AzureDevOpsConfig
{
    public string Organization { get; set; } = string.Empty;
    public string Project { get; set; } = string.Empty;
    public string PersonalAccessToken { get; set; } = string.Empty;
    public string RepositoriesUrl { get { return $"{_baseUrl}/git/repositories"; } }//?api-version=7.1-preview.1
    public string PipelinesUrl { get {return $"{_baseUrl}/pipelines"; } }//?api-version=7.1-preview
    private string _baseUrl => $"https://dev.azure.com/{Organization}/{Project}/_apis";    
}

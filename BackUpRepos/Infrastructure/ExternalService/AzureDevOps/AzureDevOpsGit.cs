using BackUpRepos.Model;
using BackUpRepos.Model.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BackUpRepos.Infrastructure.ExternalService.AzureDevOps;

public class AzureDevOpsGit : IAzureDevOpsGit
{
    private readonly AzureDevOpsConfig _azureDevOpsConfig;
    public AzureDevOpsGit(IOptions<AzureDevOpsConfig> azureDevOpsConfig)
    {
        _azureDevOpsConfig = azureDevOpsConfig.Value;
    }
    private async Task<List<string>> GetBranches(string repoName)
    {
        List<string> branchNames = new List<string>();
        string branchUrl = $"{_azureDevOpsConfig.RepositoriesUrl}/{repoName}/refs?filter=heads&api-version=7.1-preview.1";
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AddAuthorizationHeader(client);

            HttpResponseMessage response = await client.GetAsync(branchUrl);

            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                JObject jsonResponse = JObject.Parse(responseBody);

                foreach (var branch in jsonResponse["value"])
                {
                    branchNames.Add($"{branch["name"]}".Replace(@"refs/heads/", string.Empty));
                }
            }
            else
            {
                Console.WriteLine($"Error: {responseBody}");
            }
        }
        return branchNames;
    }
    public async Task<List<GetReposResponse>> GetRepos()
    {
        List<GetReposResponse> result = new List<GetReposResponse>();
        JsonElement? repositoryList = null;
        using (HttpClient client = new HttpClient())
        {
            AddAuthorizationHeader(client);
            HttpResponseMessage response = await client.GetAsync($"{_azureDevOpsConfig.RepositoriesUrl}?api-version=7.1-preview.1");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                repositoryList = JsonDocument.Parse(content).RootElement.GetProperty("value");
            }
            else
            {
                Console.WriteLine($"Error: {content}");
            }
        }
        if (repositoryList != null)
            foreach (var repo in repositoryList.Value.EnumerateArray())
            {

                string repoName = repo.GetProperty("name").GetString();
                List<string> branchList = await GetBranches(repoName);
                string cloneUrl = $"{repo.GetProperty("remoteUrl").GetString()}";
                result.Add(new GetReposResponse(repoName, cloneUrl, branchList));
            }
        return result;
    }
    private void AddAuthorizationHeader(HttpClient client)
    {
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($":{_azureDevOpsConfig.PersonalAccessToken}"));
        client.DefaultRequestHeaders.Add("Authorization", $"Basic {authToken}");
    }  
}

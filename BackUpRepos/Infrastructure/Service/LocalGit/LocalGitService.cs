using BackUpRepos.Infrastructure.Service.LocalFile;
using BackUpRepos.Model;
using BackUpRepos.Model.Config;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace BackUpRepos.Infrastructure.Service.LocalGit;

public class LocalGitService : ILocalGitService
{
    private readonly RepoFileStorageConfig _repoFileStorageConfig;
    private readonly IRepoFileStorageService _repoFileStorageService;
    public LocalGitService(IOptions<RepoFileStorageConfig> repoFileStorageConfig, IRepoFileStorageService repoFileStorageService)
    {
        _repoFileStorageConfig = repoFileStorageConfig.Value;
        _repoFileStorageService = repoFileStorageService;
    }
    private void AddPermissions(List<GetReposResponse> repoResponses)
    {
        foreach (var repo in repoResponses)
        {
            string repoName = repo.Name;
            foreach (var branch in repo.BranchList)
            {
                string targetDirectory = $@"{_repoFileStorageConfig.StoragePath}\{repoName}\{branch}";

                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);
                if (Directory.Exists(targetDirectory))
                {
                    Console.WriteLine($"safe.directory {targetDirectory}...");
                    CallExternalGitCommand($@"config --global --add safe.directory ""{targetDirectory}""");
                }
            }
        }
       
    }
    public async Task AddOrUpdateAsync(List<GetReposResponse> repoResponses)
    {
        AddPermissions(repoResponses);
        foreach (var repo in repoResponses)
        {
            string repoName = repo.Name;
            string cloneUrl = repo.RemoteUrl;
            string targetDirectory = $@"{_repoFileStorageConfig.StoragePath}\{repoName}";
            //  if (repoName == "UBEC.Metronic.Theme" || repoName == "99-ProjetosDescontinuados" || repoName == "UBEC.PHP.Web" || repoName == "NOT CLASSIFIED")
            // continue;
            foreach (var branch in repo.BranchList)
            {
                var processed = _repoFileStorageService.GetRepo(repoName, branch);
                if (processed != null)
                    continue;

                string branchTargetDir = $@"{targetDirectory}\{branch}";

                if (Directory.Exists(branchTargetDir) && Directory.EnumerateFileSystemEntries(branchTargetDir).Any())
                {
                    Console.WriteLine($"Updating {repoName} {branch}...");
                    // Pull latest changes using Git command
                    CallExternalGitCommand($@"-C ""{branchTargetDir}"" pull");
                }
                else
                {
                    Console.WriteLine($"Cloning {repoName} {branch}...");
                    // Clone repository using Git command
                    CallExternalGitCommand($"clone --single-branch --branch \"{branch}\" \"{cloneUrl}\" \"{branchTargetDir}\"");
                }
                _repoFileStorageService.SaveSuccessRepos(new Repo(repoName, branch));
            }
        }
    }
    private bool CallExternalGitCommand(string command)
    {
        bool isSuccess = false;
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = command;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            isSuccess = ShowExternalProcessOutPut(process);
        }
        return isSuccess;
    }
    private bool ShowExternalProcessOutPut(Process process)
    {
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        if (!string.IsNullOrEmpty(output))
            Console.WriteLine(output);
        if (!string.IsNullOrEmpty(error))
            Console.WriteLine($"Error: {error}");
        return string.IsNullOrEmpty(error) && process.ExitCode == 0;
    }
}

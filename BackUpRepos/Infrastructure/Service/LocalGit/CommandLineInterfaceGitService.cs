using BackUpRepos.Infrastructure.Service.LocalFile;
using BackUpRepos.Model;
using BackUpRepos.Model.Config;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;

namespace BackUpRepos.Infrastructure.Service.LocalGit;

public class CommandLineInterfaceGitService : ICommandLineInterfaceGitService
{
    private readonly RepoFileStorageConfig _repoFileStorageConfig;
    private readonly IRepoStorageService _repoFileStorageService;
    public CommandLineInterfaceGitService(IOptions<RepoFileStorageConfig> repoFileStorageConfig, IRepoStorageService repoFileStorageService)
    {
        _repoFileStorageConfig = repoFileStorageConfig.Value;
        _repoFileStorageService = repoFileStorageService;
    }
    private void AddPermissions(List<GetReposResponse> repoResponses)
    {
        var safeDirectories = CallExternalGitCommand($@"config --global --get-all safe.directory", false);
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
                    string baseCommand = $@"safe.directory ""{targetDirectory}""";

                    if (!safeDirectories.Contains(targetDirectory))
                    {
                        Console.WriteLine($"safe.directory {targetDirectory}...");
                        CallExternalGitCommand($@"config --global --add {baseCommand}");
                    }
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
    private List<string> CallExternalGitCommand(string command, bool showResult = true)
    {
        List<string> outputLines = new List<string>();
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = command;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            outputLines = GetExternalProcessOutPut(process, showResult);
            process.WaitForExit();
        }
        return outputLines;
    }
    private List<string> GetExternalProcessOutPut(Process process, bool showResult = true)
    {
        var outputLines = new List<string>();
        using (StreamReader reader = new StreamReader(process.StandardOutput.BaseStream, Encoding.UTF8))
        {
            string line;
            while (!string.IsNullOrEmpty(line = $"{reader.ReadLine()}"))
            {
                if (showResult)
                    Console.WriteLine(line);
                outputLines.Add(line);
            }
        }
        string error = process.StandardError.ReadToEnd();
        if (!string.IsNullOrEmpty(error))
            Console.WriteLine($"Error: {error}");
        return outputLines;
    }
}

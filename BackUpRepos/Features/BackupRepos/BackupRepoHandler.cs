using BackUpRepos.Infrastructure.Service.LocalGit;
using MediatR;

namespace BackUpRepos.Features.BackupRepos;

public class BackupRepoHandler : IRequestHandler<BackupRepoRequest>
{
    private readonly ILocalGitService _localGitService;
    public BackupRepoHandler(ILocalGitService localGitService)
    {
        _localGitService = localGitService;
    }
    public async Task Handle(BackupRepoRequest request, CancellationToken cancellationToken)
    {
         await _localGitService.AddOrUpdateAsync(request.Repos);      
    }
}


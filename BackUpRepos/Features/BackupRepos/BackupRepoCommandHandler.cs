using BackUpRepos.Infrastructure.Service.LocalGit;
using MediatR;

namespace BackUpRepos.Features.BackupRepos;

public class BackupRepoCommandHandler : IRequestHandler<BackupRepoCommand>
{
    private readonly ICommandLineInterfaceGitService _localGitService;
    public BackupRepoCommandHandler(ICommandLineInterfaceGitService localGitService)
    {
        _localGitService = localGitService;
    }
    public async Task Handle(BackupRepoCommand request, CancellationToken cancellationToken)
    {
         await _localGitService.AddOrUpdateAsync(request.Repos);      
    }
}


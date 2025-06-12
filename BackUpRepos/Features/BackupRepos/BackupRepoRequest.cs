using BackUpRepos.Model;
using MediatR;

namespace BackUpRepos.Features.BackupRepos;

public class BackupRepoRequest : IRequest
{
    public List<GetReposResponse> Repos { get; set; } = new List<GetReposResponse>();
}

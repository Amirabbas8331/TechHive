
namespace TechHive.Application.Common;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}

using System.Threading.Tasks;

namespace DevRating.Domain.Git
{
    public interface Commit
    {
        string Sha();
        string RepositoryFirstUrl();
        string Author();
        Task WriteInto(ModificationsCollection modifications);
    }
}
using System.Threading.Tasks;

namespace DevRating.Domain.Git
{
    public interface Commit
    {
        string Sha();
        string RepositoryFirstUrl();
        string AuthorEmail();
        Task WriteInto(ModificationsCollection modifications);
    }
}
using System.Threading.Tasks;

namespace DevRating.Git
{
    public interface AuthorChangesCollection
    {
        Task ExtendAuthorChanges(AuthorChanges changes, string empty);
    }
}
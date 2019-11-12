using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevRating.Domain
{
    public interface Commit
    {
        string Sha();
        string RepositoryFirstUrl();
        string AuthorEmail();
        Task WriteInto(IList<Addition> additions, IList<Deletion> deletions);
    }
}
using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetAuthorOperation
    {
        Author Author(string email);
        Author Author(Id id);
        IEnumerable<Author> Top();
        IEnumerable<Author> Top(string repository);
    }
}
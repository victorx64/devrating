using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetAuthorOperation
    {
        Author Author(string organization, string email);
        Author Author(Id id);
        IEnumerable<Author> TopOfOrganization(string organization);
        IEnumerable<Author> TopOfRepository(string repository);
    }
}
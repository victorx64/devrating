using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public interface AuthorChange
    {
        IDictionary<string, Player> UpdatedAuthors(IDictionary<string, Player> authors, Player initial);
    }
}
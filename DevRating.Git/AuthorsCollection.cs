using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public interface AuthorsCollection
    {
        IDictionary<string, Player> Authors();
    }
}
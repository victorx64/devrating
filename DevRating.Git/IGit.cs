using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public interface IGit
    {
        IDictionary<string, IPlayer> Authors();
    }
}
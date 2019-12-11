using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Work
    {
        uint Additions();
        Author Author();
        IEnumerable<Rating> Ratings();
        Rating UsedRating();
        bool HasUsedRating();
    }
}
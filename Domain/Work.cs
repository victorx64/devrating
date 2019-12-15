using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Work : IdObject
    {
        uint Additions();
        Author Author();
        IEnumerable<Rating> Ratings();
        Rating UsedRating();
        bool HasUsedRating();
    }
}
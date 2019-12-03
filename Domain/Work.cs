using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Work
    {
        double Reward();
        Author Author();
        IEnumerable<Rating> Ratings();
        Rating UsedRating();
        bool HasUsedRating();
    }
}
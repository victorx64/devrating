using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Work
    {
        double Reward();
        string Author();
        IEnumerable<RatingUpdate> RatingUpdates();
    }
}
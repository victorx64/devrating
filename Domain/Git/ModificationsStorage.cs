using System.Collections.Generic;

namespace DevRating.Domain.Git
{
    public interface ModificationsStorage
    {
        void InsertAdditions(IEnumerable<Addition> additions);
        void InsertDeletions(IEnumerable<Deletion> deletions);
        IEnumerable<Reward> RewardsOf(Commit commit);
        IEnumerable<Rating> RatingsOf(Commit commit);
    }
}
using System;

namespace DevRating.Domain
{
    public interface InsertRatingOperation
    {
        Rating Insert(double value, Envelope deletions, Id previous, Id work, Id author);
    }
}
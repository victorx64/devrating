namespace DevRating.Domain
{
    public interface GetRatingOperation
    {
        Rating RatingOf(Entity author);
        Rating Rating(object id);
    }
}
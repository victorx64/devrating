namespace DevRating.Domain
{
    public interface RatingGetOperation
    {
        Rating RatingOf(Entity author);
        Rating Rating(object id);
    }
}
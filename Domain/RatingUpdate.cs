namespace DevRating.Domain
{
    public interface RatingUpdate
    {
        string Author();
        double OldRating();
        double NewRating();
    }
}
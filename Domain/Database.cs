namespace DevRating.Domain
{
    public interface Database
    {
        DbInstance Instance();
        Works Works();
        Ratings Ratings();
        Authors Authors();
    }
}
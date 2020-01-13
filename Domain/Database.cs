namespace DevRating.Domain
{
    public interface Database
    {
        DbInstance Instance();
        Entities Entities();
    }
}
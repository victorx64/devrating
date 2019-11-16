namespace DevRating.SqlClient
{
    internal interface Rating : IdentifiableObject
    {
        double Value();
        bool HasPreviousRating();
        Rating PreviousRating();
        IdentifiableWork Work();
        Author Author();
    }
}
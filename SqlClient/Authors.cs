namespace DevRating.SqlClient
{
    internal interface Authors
    {
        Author Insert(string email);
        bool Exist(string email);
        Author Author(string email);
    }
}
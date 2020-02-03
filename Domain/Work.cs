namespace DevRating.Domain
{
    public interface Work : Entity
    {
        uint Additions();
        Author Author();
        Rating UsedRating();
        string Repository();
        string Start();
        string End();
    }
}
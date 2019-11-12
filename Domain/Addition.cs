namespace DevRating.Domain
{
    public interface Addition : Modification
    {
        Addition NewAddition(uint count);
    }
}
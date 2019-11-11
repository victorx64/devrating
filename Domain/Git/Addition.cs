namespace DevRating.Domain.Git
{
    public interface Addition : Modification
    {
        Addition NewAddition(uint count);
    }
}
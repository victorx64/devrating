namespace DevRating.Vcs
{
    public interface Addition : Modification
    {
        Addition NewAddition(int count);
    }
}
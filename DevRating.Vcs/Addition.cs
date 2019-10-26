namespace DevRating.Vcs
{
    public interface Addition : Modification
    {
        Addition UpdatedAddition(int count);
    }
}
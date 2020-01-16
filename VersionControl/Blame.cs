namespace DevRating.VersionControl
{
    public interface Blame
    {
        string AuthorEmail();
        uint StartLineNumber();
        uint LineCount();
        bool ContainsLine(uint line);
    }
}
namespace DevRating.VersionControl
{
    public interface Blame
    {
        string AuthorEmail();
        uint FinalStartLineNumber();
        uint LineCount();
    }
}
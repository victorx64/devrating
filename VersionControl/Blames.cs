namespace DevRating.VersionControl
{
    public interface Blames
    {
        Blame HunkForLine(uint line);
    }
}
namespace DevRating.VersionControl
{
    public interface Blames
    {
        Blame HunkForLine(int line);
    }
}
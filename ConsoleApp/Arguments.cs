namespace DevRating.ConsoleApp
{
    internal interface Arguments
    {
        string Path();
        string StartCommit();
        string EndCommit();
        string Command();
    }
}
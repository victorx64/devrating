using System.IO;

namespace DevRating.Git
{
    public interface Process
    {
        StreamReader Output(string name, string arguments);
    }
}
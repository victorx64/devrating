using System.IO;

namespace DevRating.Git
{
    public interface Process
    {
        string Output(string name, string arguments);
    }
}
using System.IO;

namespace DevRating
{
    public interface IProcess
    {
        StreamReader Output(string name, string arguments);
    }
}
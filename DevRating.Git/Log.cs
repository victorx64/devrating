using System.Threading.Tasks;

namespace DevRating.Git
{
    public interface Log
    {
        void LogDeletion(string victim);
        void LogAddition();
    }
}
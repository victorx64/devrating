using System.Threading.Tasks;

namespace DevRating.Git
{
    public interface Log
    {
        Task LogDeletion(int count, string victim, string initiator, string commit);
        Task LogAddition(int count, string initiator, string commit);
    }
}
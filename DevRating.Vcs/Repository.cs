using System.Threading.Tasks;

namespace DevRating.Vcs
{
    public interface Repository
    {
        Task WriteInto(ModificationsCollection modifications, string sha);
    }
}
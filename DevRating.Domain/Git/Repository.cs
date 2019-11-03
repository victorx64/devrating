using System.Threading.Tasks;

namespace DevRating.Domain.Git
{
    public interface Repository
    {
        Task WriteInto(ModificationsCollection modifications, string sha);
    }
}
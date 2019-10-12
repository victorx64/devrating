using System.Threading.Tasks;

namespace DevRating.Git
{
    public interface Repository
    {
        Task WriteInto(Modifications modifications, string sha);
    }
}
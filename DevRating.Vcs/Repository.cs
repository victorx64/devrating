using System.Threading.Tasks;

namespace DevRating.Vcs
{
    public interface Repository
    {
        Task WriteInto(Modifications modifications, string sha);
    }
}
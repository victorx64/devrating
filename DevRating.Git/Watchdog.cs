using System.Threading.Tasks;

namespace DevRating.Git
{
    public interface Watchdog
    {
        Task WriteInto(Modifications modifications);
    }
}
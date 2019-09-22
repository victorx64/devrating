using System.Threading.Tasks;

namespace DevRating.Git
{
    internal interface Watchdog
    {
        Task WriteInto(Modifications modifications);
    }
}
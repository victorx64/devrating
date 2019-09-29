using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevRating.Game
{
    public interface Matches
    {
        Task<double> Points(string player);
        Task Add(string player, string contender, string commit, double points, double reward, int rounds);
        Task Add(string player, string commit, double points, double reward, int rounds);
        Task<IEnumerable<Match>> Matches(string player);
        Task Lock(string player);
        Task Unlock(string player);
        Task Sync();
        string Report(string commit);
    }
}
using System.Collections.Generic;

namespace DevRating.Game
{
    public interface Player
    {
        Player PerformedPlayer(string contender, string commit, double points, double reward, int rounds);
        double Points();
        IEnumerable<Game> Games();
    }
}
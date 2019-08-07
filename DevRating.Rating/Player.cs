using System;

namespace DevRating.Rating
{
    public interface Player : IComparable<Player>
    {
        double Points();
        int Games();
        Player Winner(Player opponent);
        Player Loser(Player opponent);
        void Print(Output output);
    }
}
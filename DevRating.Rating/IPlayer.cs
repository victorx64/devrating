using System;

namespace DevRating.Rating
{
    public interface IPlayer : IComparable<IPlayer>
    {
        double Points();
        int Games();
        IPlayer Winner(IPlayer opponent);
        IPlayer Loser(IPlayer opponent);
        void Print(IOutput output);
    }
}
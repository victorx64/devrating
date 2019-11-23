using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    internal interface Application
    {
        void Top();
        void Reset();
        void Save(Diff diff);
        void PrintToConsole(Diff diff);
    }
}
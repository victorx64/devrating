using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    internal interface Application
    {
        void Top(Console console, string organization);
        void Save(Diff diff);
        void PrintTo(Console console, Diff diff);
    }
}
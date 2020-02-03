using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    internal interface Application
    {
        void Top(string organization);
        void Save(Diff diff);
        void PrintToConsole(Diff diff);
    }
}
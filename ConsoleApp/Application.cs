namespace DevRating.ConsoleApp
{
    internal interface Application
    {
        void Top();
        void Save(string path, string start, string end);
        void PrintToConsole(string path, string start, string end);
    }
}
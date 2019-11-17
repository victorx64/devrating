namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var command = args[0];
            var path = args[1];
            var start = args[2];
            var end = args[3];
            var database =
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DevRating;" +
                @"Integrated Security=True;Persist Security Info=False;Pooling=False;" +
                @"MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";

            var application = new Application(path, start, end, database);

            if (command.Equals("show"))
            {
                application.PrintToConsole();
            }

            if (command.Equals("show-saved"))
            {
                application.PrintSavedToConsole();
            }

            if (command.Equals("save"))
            {
                application.Save();
            }
        }
    }
}
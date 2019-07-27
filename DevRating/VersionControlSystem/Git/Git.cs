using System;
using System.Linq;
using DevRating.Rating;

namespace DevRating.VersionControlSystem.Git
{
    public class Git : IVersionControlSystem
    {
        private readonly IProcess _process;

        public Git(IProcess process)
        {
            _process = process;
        }
        
        public IRating Rating()
        {
            var log = new Log(_process);

            var files = log.ModifiedFiles();

            var index = 0;

            var length = files.Count();
            
            IRating rating = new Rating.Elo.Rating();
            
            foreach (var file in files)
            {
                Console.Write($"{index++} / {length} ");

                file.PrintToConsole();
                
                var modifications = file.Modifications();
                
                foreach (var modification in modifications)
                {
                    rating = modification.UpdatedRating(rating);
                }
            }
            
            return rating;
        }
    }
}
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
        
        public IRating UpdatedRating(IRating rating)
        {
            var log = new Log(_process);

            var commits = log.Commits();

            foreach (var commit in commits)
            {
                var files = commit.FileUpdates();

                foreach (var file in files)
                {
                    var lines = file.RemovedLines();

                    foreach (var line in lines)
                    {
                        rating = line.UpdatedRating(rating);
                    }
                }
            }

            return rating;
        }
    }
}
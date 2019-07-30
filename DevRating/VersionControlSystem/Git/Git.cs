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

            var blobs = log.ModifiedBlobs();

            IRating rating = new Rating.Elo.Rating();
            
            foreach (var blob in blobs)
            {
                rating = blob.UpdateRating(rating);
            }
            
            return rating;
        }
    }
}
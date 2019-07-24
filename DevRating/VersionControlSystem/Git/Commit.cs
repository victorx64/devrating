using System.Collections.Generic;

namespace DevRating.VersionControlSystem.Git
{
    public class Commit
    {
        private readonly string _information;

        public Commit(string information)
        {
            _information = information;
        }

        public ICollection<FileUpdate> FileUpdates()
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Collections.Generic;

namespace DevRating.VersionControlSystem.Git
{
    public class FileUpdate
    {
        private readonly string _commitHash;
        private readonly string _filename;
        private readonly int _removeIndex;
        private readonly int _removeLength;
        private readonly string _authorEmail;

        public FileUpdate(string information)
        {
            
        }

        public FileUpdate(string commitHash, string filename, int removeIndex, int removeLength,
            string authorEmail)
        {
            _commitHash = commitHash;
            _filename = filename;
            _removeIndex = removeIndex;
            _removeLength = removeLength;
            _authorEmail = authorEmail;
        }

        public ICollection<RemovedLine> RemovedLines()
        {
            throw new System.NotImplementedException();
        }
    }
}
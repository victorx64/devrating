using System.Collections.Generic;

namespace DevRating.Git.Test
{
    public class FakeModifications : Modifications
    {
        private readonly IList<string> _deletions = new List<string>();
        private int _additions;
        
        public void AddDeletion(string victim)
        {
            lock (_deletions)
            {
                _deletions.Add(victim);
            }
        }

        public void AddAdditions(int count)
        {
            lock (_deletions)
            {
                _additions += count;    
            }
        }

        public IEnumerable<string> Deletions()
        {
            return _deletions;
        }

        public int Additions()
        {
            return _additions;
        }
    }
}
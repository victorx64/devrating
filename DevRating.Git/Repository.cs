using System.Collections.Generic;

namespace DevRating.Git
{
    public class Repository
    {
        private readonly IEnumerable<Patch> _patches;

        public Repository(IEnumerable<Patch> patches)
        {
            _patches = patches;
        }
    }
}
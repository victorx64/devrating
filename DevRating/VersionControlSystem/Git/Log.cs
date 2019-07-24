using System;
using System.Collections.Generic;

namespace DevRating.VersionControlSystem.Git
{
    public class Log
    {
        private readonly IProcess _process;

        public Log(IProcess process)
        {
            _process = process;
        }

        public ICollection<Commit> Commits()
        {
            var output = _process.Output("git", "log -U0 --no-merges --pretty=oneline");
            
            throw new NotImplementedException();
        }
    }
}
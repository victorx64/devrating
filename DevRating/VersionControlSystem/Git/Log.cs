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

        public IEnumerable<File> ModifiedFiles()
        {
            Console.WriteLine("Fetching git log...");
            
            var stream = _process.Output("git", "--no-pager log --reverse --no-merges -U0 --pretty=format:commit,%H,%aE");

            string hash = string.Empty,
                author = string.Empty,
                path = string.Empty;
            
            var blocks = new List<LinesBlock>();

            var fileUpdates = new List<File>();

            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine();

                if (line.StartsWith("commit,"))
                {
                    var parts = line.Split(',');

                    hash = parts[1];

                    author = parts[2];
                }
                else if (line.StartsWith("--- "))
                {
                    if (blocks.Count > 0)
                    {
                        fileUpdates.Add(new File(_process, hash, path, author, blocks));
                    
                        blocks = new List<LinesBlock>();
                    }
                    
                    path = line.Substring(6);
                }
                else if (line.StartsWith("@@ "))
                {
                    var parts = line.Split(' ');

                    var deletions = parts[1].Substring(1).Split(',');

                    var index = Convert.ToInt32(deletions[0]);

                    var length = deletions.Length > 1 ? Convert.ToInt32(deletions[1]) : 1;

                    if (length > 0)
                    {
                        blocks.Add(new LinesBlock(index, length));
                    }
                }
            }

            return fileUpdates;
        }
    }
}
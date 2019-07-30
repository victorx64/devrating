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

        public IEnumerable<Blob> ModifiedBlobs()
        {
            var stream = _process.Output("git",
                "--no-pager log --reverse -U0 --pretty=format:commit,%aE --full-index --use-mailmap");

            const string initial = "0000000000000000000000000000000000000000";

            string author = string.Empty,
                previous = string.Empty,
                next = string.Empty;

            var deletions = new List<LinesBlock>();
            var additions = new List<LinesBlock>();


            var blobs = new Dictionary<string, Blob> {{initial, new Blob()}};

            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine();

                if (line.StartsWith("commit,"))
                {
                    author = line.Substring("commit,".Length);
                }
                else if (line.StartsWith("index "))
                {
                    if (blobs.ContainsKey(previous)) // check if blob has a 'new file mode' commit
                    {
                        if (next.Equals(initial))
                        {
                            next = MaskDeletedBlobHash(previous);
                        }

                        if (!blobs.ContainsKey(next)) // some blobs can be created multiple times. we ignore them
                        {
                            var authors = blobs[previous].Authors();

                            blobs.Add(next, new Blob(authors, deletions, additions));
                        }
                    }

                    deletions = new List<LinesBlock>();
                    additions = new List<LinesBlock>();

                    var parts = line.Split(' ', '.');

                    previous = parts[1];

                    next = parts[3];
                }
                else if (line.StartsWith("@@ "))
                {
                    var parts = line.Split(' ');

                    deletions.Add(new LinesBlock(author, parts[1]));
                    additions.Add(new LinesBlock(author, parts[2]));
                }
            }

            return blobs.Values;
        }

        private string MaskDeletedBlobHash(string previousHash)
        {
            return "deleted-" + previousHash;
        }
    }
}
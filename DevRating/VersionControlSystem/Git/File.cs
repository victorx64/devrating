using System;
using System.Collections.Generic;
using System.IO;

namespace DevRating.VersionControlSystem.Git
{
    public class File
    {
        private readonly IProcess _process;
        
        private readonly string _hash;
        
        private readonly string _path;
        
        private readonly string _author;
        
        private readonly List<LinesBlock> _blocks;

        public File(IProcess process, string hash, string path, string author, List<LinesBlock> blocks)
        {
            _process = process;
            
            _hash = hash;
            
            _path = path;
            
            _author = author;
            
            _blocks = blocks;
        }

        public IEnumerable<Modification> Modifications()
        {
            var modifications = new List<Modification>();

            var stream = BlameStream();

            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine();
                
                try
                {
                    var metadata = Metadata(line);

                    var author = Author(metadata);

                    var index = LineIndex(metadata);

                    var removed = Removed(index);

                    if (removed)
                    {
                        modifications.Add(new Modification(author, _author));
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Couldn't parse line: {line}");
                }
            }

            return modifications;
        }

        public void PrintToConsole()
        {
            Console.WriteLine($"{_hash} {_author} {_path}");
        }

        private int LineIndex(string[] metadata)
        {
            return Convert.ToInt32(metadata[metadata.Length - 1]);
        }

        private string Author(string[] metadata)
        {
            return metadata[0].Trim('<', '>');
        }

        private StreamReader BlameStream()
        {
            _blocks.Sort();

            var start = _blocks[0].Start();

            var end = _blocks[_blocks.Count - 1].End();

            return _process.Output("git", $"blame -t -L {start},{end} -e {_hash}~1 -- {_path}");
        }

        private bool Removed(int lineIndex)
        {
            foreach (var block in _blocks)
            {
                if (block.InRange(lineIndex))
                {
                    return true;
                }
            }

            return false;
        }

        private string[] Metadata(string line)
        {
            var start = line.IndexOf('(');

            var end = line.IndexOf(')');

            var length = end - start - 1;

            var metadata = line.Substring(start + 1, length);
            
            return metadata.Split(' ');
        }
    }
}
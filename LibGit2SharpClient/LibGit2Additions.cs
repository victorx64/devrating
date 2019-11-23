using System;

namespace DevRating.LibGit2SharpClient
{
    internal sealed class LibGit2Additions : Additions
    {
        private readonly string _patch;

        public LibGit2Additions(string patch)
        {
            _patch = patch;
        }

        public uint Count()
        {
            var additions = 0u;

            foreach (var line in _patch.Split('\n'))
            {
                if (line.StartsWith("@@ "))
                {
                    var parts = line.Split(' ');

                    additions += AdditionsCount(parts[2]);
                }
            }

            return additions;
        }

        private uint AdditionsCount(string header)
        {
            var parts = header
                .Substring(1)
                .Split(',');

            return parts.Length == 1 ? 1 : Convert.ToUInt32(parts[1]);
        }
    }
}
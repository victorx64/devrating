using System;
using DevRating.Domain;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Additions : Additions
    {
        private readonly string _patch;

        public LibGit2Additions(string patch)
        {
            _patch = patch;
        }

        public uint Count()
        {
            var dangerous = false;
            var additions = 0u;

            foreach (var line in _patch.Split('\n'))
            {
                if (line.StartsWith("@@ "))
                {
                    additions += AdditionsCount(line.Split(' ')[2]);
                    dangerous = true;
                }
                else if (dangerous)
                {
                    if (line.StartsWith("+") || line.StartsWith("-"))
                    {
                        dangerous = false;
                    }
                    else
                    {
                        throw new EncounteredNonContextualLineException();
                    }
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
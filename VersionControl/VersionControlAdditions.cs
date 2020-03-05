// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DevRating.VersionControl
{
    public sealed class VersionControlAdditions : Additions
    {
        private readonly string _patch;

        public VersionControlAdditions(string patch)
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
                        throw new ContextLineEncounteredException();
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
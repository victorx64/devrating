// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeBlames : Blames
    {
        private readonly IEnumerable<Blame> _blames;

        public FakeBlames(IEnumerable<Blame> blames)
        {
            _blames = blames;
        }

        public Blame HunkForLine(uint line)
        {
            bool ContainsLine(Blame x)
            {
                return x.ContainsLine(line);
            }

            return _blames.Single(ContainsLine);
        }
    }
}
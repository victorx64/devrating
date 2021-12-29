// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeFileBlames : AFileBlames
    {
        private readonly IEnumerable<Blame> _blames;

        public FakeFileBlames() : this(new VersionControlBlame[0])
        {
        }

        public FakeFileBlames(IEnumerable<Blame> blames)
        {
            _blames = blames;
        }

        public Blame AtLine(uint line)
        {
            bool predicate(Blame b)
            {
                return b.ContainsLine(line);
            }

            return _blames.Single(predicate);
        }
    }
}
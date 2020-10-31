// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeFileBlames : AFileBlames
    {
        private readonly IEnumerable<(uint Start, uint Count, Blame Blame)> _blames;

        public FakeFileBlames() : this (new (string Email, uint Start, uint Count)[0])
        {
        }

        public FakeFileBlames(IEnumerable<(string Email, uint Start, uint Count)> blames)
            : this(
                  blames.Select(
                      b => (
                          b.Start,
                          b.Count,
                          (Blame)new FakeBlame(b.Email, b.Start, b.Count)
                      )
                  )
              )
        {
        }

        public FakeFileBlames(IEnumerable<(uint Start, uint Count, Blame Blame)> blames)
        {
            _blames = blames;
        }

        public Blame AtLine(uint line)
        {
            bool ContainsLine((uint Start, uint Count, Blame Blame) x)
            {
                return x.Start <= line && line < x.Start + x.Count;
            }

            return _blames.Single(ContainsLine).Blame;
        }
    }
}
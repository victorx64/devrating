// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DevRating.ConsoleApp.Fake
{
    public sealed class FakeOutput : Output
    {
        private readonly IList<string> _lines;

        public FakeOutput(IList<string> lines)
        {
            _lines = lines;
        }

        public void WriteLine()
        {
            _lines.Add(string.Empty);
        }

        public void WriteLine(string value)
        {
            _lines.Add(value);
        }
    }
}
// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeProcess : Process
    {
        private readonly IList<string> _output;

        public FakeProcess(string output)
            : this (output.Split('\n'))
        {
        }

        public FakeProcess(IList<string> output)
        {
            _output = output;
        }

        public IList<string> Output()
        {
            return _output;
        }
    }
}
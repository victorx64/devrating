// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Xunit;

namespace DevRating.ConsoleApp.Test
{
    public sealed class SystemConsoleTest
    {
        [Fact]
        public void WritesLine()
        {
            var stream = new StringWriter();

            System.Console.SetOut(stream);

            var expected = "123";

            new SystemConsole().WriteLine(expected);

            Assert.Equal(expected + Environment.NewLine, stream.ToString());
        }

        [Fact]
        public void WritesEmptyLine()
        {
            var stream = new StringWriter();

            System.Console.SetOut(stream);

            new SystemConsole().WriteLine();

            Assert.Equal(Environment.NewLine, stream.ToString());
        }
    }
}
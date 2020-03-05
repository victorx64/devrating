// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DevRating.ConsoleApp
{
    public sealed class SystemConsole : Console
    {
        public void WriteLine()
        {
            System.Console.WriteLine();
        }

        public void WriteLine(string value)
        {
            System.Console.WriteLine(value);
        }
    }
}
// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DevRating.ConsoleApp
{
    public interface Output
    {
        void WriteLine();
        void WriteLine(string value);
    }
}
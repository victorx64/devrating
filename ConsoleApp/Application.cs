// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    public interface Application
    {
        void Top(Output output, string organization);
        void Save(Diff diff);
        void PrintTo(Output output, Diff diff);
    }
}
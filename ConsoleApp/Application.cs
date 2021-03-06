// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    public interface Application
    {
        void Top(Output output, string organization, string repository);
        void Total(Output output, string organization, string repository, DateTimeOffset after);
        void Save(Diff diff);
        void PrintTo(Output output, Diff diff);
    }
}
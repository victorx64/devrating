// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Formula
    {
        double DefaultRating();
        double WinnerNewRating(double current, IEnumerable<Match> matches);
        double LoserNewRating(double current, Match match);
        double WinProbabilityOfA(double a, double b);
    }
}
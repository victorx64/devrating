// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DevRating.VersionControl
{
    public sealed class VersionControlProcess : Process
    {
        private readonly System.Diagnostics.ProcessStartInfo _info;

        public VersionControlProcess(string filename, string arguments, string directory)
            : this (new System.Diagnostics.ProcessStartInfo(filename, arguments)
                {
                    WorkingDirectory = directory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true
                }
            )
        {
        }

        public VersionControlProcess(System.Diagnostics.ProcessStartInfo info)
        {
            _info = info;
        }

        public IList<string> Output()
        {
            var process = System.Diagnostics.Process.Start(_info)
                ?? throw new InvalidOperationException("Process.Start() returned null");

            var output = process.StandardOutput
                .ReadToEnd()
                .Split(Environment.NewLine);

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException(process.StandardError.ReadToEnd());
            }

            return output;
        }
    }
}
using System;
using System.Diagnostics;
using System.IO;

namespace DevRating
{
    public class Process : IProcess
    {
        private readonly IWorkingDirectory _workingDirectory;

        public Process(IWorkingDirectory workingDirectory)
        {
            _workingDirectory = workingDirectory;
        }

        public StreamReader Output(string name, string arguments)
        {
            var info = new ProcessStartInfo(name, arguments)
            {
                RedirectStandardOutput = true,

                RedirectStandardError = true,

                RedirectStandardInput = true,

                WorkingDirectory = _workingDirectory.ToString()
            };

            var process = System.Diagnostics.Process.Start(info);

            if (process == null)
            {
                throw new Exception("Process.Start(info) returned null");
            }

            return process.StandardOutput;
        }
    }
}
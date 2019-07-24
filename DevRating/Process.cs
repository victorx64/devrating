using System;
using System.Diagnostics;

namespace DevRating
{
    public class Process : IProcess
    {
        public string Output(string name, string arguments)
        {
            var info = new ProcessStartInfo(name, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = System.Diagnostics.Process.Start(info);

            if (process == null)
            {
                throw new Exception("Process.Start(info) returned null");
            }
            
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception(process.StandardError.ReadToEnd());
            }

            return process.StandardOutput.ReadToEnd();
        }
    }
}
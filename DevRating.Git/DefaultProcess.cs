using System;
using System.Diagnostics;

namespace DevRating.Git
{
    public class DefaultProcess : Process
    {
        public string Output(string name, string arguments)
        {
            var info = new ProcessStartInfo(name, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            };

            var process = System.Diagnostics.Process.Start(info);

            if (process == null)
            {
                throw new Exception("Process.Start(info) returned null");
            }

            var output = process.StandardOutput.ReadToEnd();
            
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception(process.StandardError.ReadToEnd());
            }

            return output;
        }
    }
}
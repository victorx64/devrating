using System.Diagnostics;

namespace devrating.git;

public sealed class GitProcess : Process
{
    private readonly ProcessStartInfo _info;

    public GitProcess(string filename, string arguments, string directory)
        : this(new ProcessStartInfo(filename, arguments)
        {
            WorkingDirectory = directory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true
        }
        )
    {
    }

    public GitProcess(ProcessStartInfo info)
    {
        _info = info;
    }

    public IList<string> Output()
    {
        var process = System.Diagnostics.Process.Start(_info)
            ?? throw new InvalidOperationException("Process.Start() returned null");

        var output = process.StandardOutput
            .ReadToEnd()
            .Split('\n');

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(process.StandardError.ReadToEnd());
        }

        return output;
    }
}

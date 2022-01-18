using System.Diagnostics;
using devrating.factory;

namespace devrating.git;

public sealed class GitProcess : Process
{
    private readonly ProcessStartInfo _info;
    private readonly Log _log;

    public GitProcess(Log log, string filename, string arguments, string directory)
        : this(
            log,
            new ProcessStartInfo(filename, arguments)
            {
                WorkingDirectory = directory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            }
        )
    {
    }

    public GitProcess(Log log, ProcessStartInfo info)
    {
        _info = info;
        _log = log;
    }

    public IList<string> Output()
    {
        _log.WriteLine($"{ToString()}");

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

    public override string ToString()
    {
        return $"info-07737698: WorkingDir: `{_info.WorkingDirectory}`, FileName: `{_info.FileName}`, Args: `{_info.Arguments}`";
    }
}

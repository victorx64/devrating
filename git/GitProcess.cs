using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace devrating.git;

public sealed class GitProcess : Process
{
    private readonly ProcessStartInfo _info;
    private readonly ILogger _log;

    public GitProcess(ILoggerFactory log, string filename, string arguments, string directory)
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

    public GitProcess(ILoggerFactory log, ProcessStartInfo info)
    {
        _info = info;
        _log = log.CreateLogger(this.GetType().ToString());
    }

    public IList<string> Output()
    {
        _log.LogInformation(new EventId(1770264), ToString());

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
        return $"`{_info.WorkingDirectory}` `{_info.FileName}` `{_info.Arguments}`";
    }
}

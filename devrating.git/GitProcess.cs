namespace devrating.git;

public sealed class GitProcess : Process
{
    private readonly System.Diagnostics.ProcessStartInfo _info;

    public GitProcess(string filename, string arguments, string directory)
        : this(new System.Diagnostics.ProcessStartInfo(filename, arguments)
        {
            WorkingDirectory = directory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true
        }
        )
    {
    }

    public GitProcess(System.Diagnostics.ProcessStartInfo info)
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

using System.Collections.Generic;
using System.Diagnostics;

namespace GitCredentialManager.Tests;

public class TestProcessManager : IProcessManager
{
    public IList<ProcessStartInfo> CreatedProcesses { get; set; } = new List<ProcessStartInfo>();

    public Process CreateProcess(string path, string args, bool useShellExecute, string workingDirectory)
    {
        var psi = new ProcessStartInfo(path, args)
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = useShellExecute,
            WorkingDirectory = workingDirectory ?? string.Empty
        };

        CreatedProcesses.Add(psi);

        return new Process { StartInfo = psi };
    }
}
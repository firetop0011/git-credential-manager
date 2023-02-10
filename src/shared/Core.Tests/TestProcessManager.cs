using System;
using System.Collections.Generic;
using System.Diagnostics;
using GitCredentialManager.Tests.Objects;
using Moq;

namespace GitCredentialManager.Tests;

public class TestProcessManager : IProcessManager
{
    public ChildProcess CreateProcess(string path, string args, bool useShellExecute, string workingDirectory)
    {
        var psi = new ProcessStartInfo(path, args)
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = useShellExecute,
            WorkingDirectory = workingDirectory ?? string.Empty
        };

        return new ChildProcess(new NullTrace2(), psi);
    }
}
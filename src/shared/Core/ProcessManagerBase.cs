using System.Diagnostics;

namespace GitCredentialManager;

public interface IProcessManager
{
    /// <summary>
    /// Create a process ready to start, with redirected streams.
    /// </summary>
    /// <param name="path">Absolute file path of executable or command to start.</param>
    /// <param name="args">Command line arguments to pass to executable.</param>
    /// <param name="useShellExecute">
    /// True to resolve <paramref name="path"/> using the OS shell, false to use as an absolute file path.
    /// </param>
    /// <param name="workingDirectory">Working directory for the new process.</param>
    /// <returns><see cref="Process"/> object ready to start.</returns>
    Process CreateProcess(string path, string args, bool useShellExecute, string workingDirectory);
}

public class ProcessManagerBase : IProcessManager
{
    public virtual Process CreateProcess(string path, string args, bool useShellExecute, string workingDirectory)
    {
        var psi = new ProcessStartInfo(path, args)
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = useShellExecute,
            WorkingDirectory = workingDirectory ?? string.Empty
        };

        return new Process { StartInfo = psi };
    }
}

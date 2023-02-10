using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GitCredentialManager;

public class ChildProcess : DisposableObject
{
    private readonly ITrace2 _trace2;

    private DateTimeOffset _startTime;
    private DateTimeOffset _exitTime => Process.ExitTime;

    private int _id => Process.Id;

    public Process Process { get; }
    public StreamWriter StandardInput => Process.StandardInput;
    public StreamReader StandardOutput => Process.StandardOutput;
    public StreamReader StandardError => Process.StandardError;
    public int Id => Process.Id;
    public int ExitCode => Process.ExitCode;

    public static ChildProcess Start(ITrace2 trace2, ProcessStartInfo startInfo)
    {
        var childProc = new ChildProcess(trace2, startInfo);
        childProc.Start();
        return childProc;
    }

    public ChildProcess(ITrace2 trace2, ProcessStartInfo startInfo)
    {
        _trace2 = trace2;
        Process = new Process() { StartInfo = startInfo };
        Process.Exited += ProcessOnExited;
    }

    public void Start()
    {
        ThrowIfDisposed();
        // Record the time just before the process starts, since:
        // (1) There is no event related to Start as there is with Exit.
        // (2) Using Process.StartTime causes a race condition that leads
        // to an exception if the process finishes executing before the
        // variable is passed to Trace2.
        _startTime = DateTimeOffset.UtcNow;
        Process.Start();
        _trace2.ChildStart(_startTime);
    }

    public void WaitForExit() => Process.WaitForExit();

    public void Kill() => Process.Kill();

    protected override void ReleaseManagedResources()
    {
        Process.Exited -= ProcessOnExited;
        Process.Dispose();
        base.ReleaseUnmanagedResources();
    }

    private void ProcessOnExited(object sender, EventArgs e)
    {
        if (sender is Process)
        {
            double elapsedTime = (_exitTime - _startTime).TotalSeconds;
            _trace2.ChildExit(elapsedTime, _id, Process.ExitCode);
        }
    }
}

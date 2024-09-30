using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Godot;

/// <summary>
/// Represents a "Thread" that can be used to process tasks in a queue.
/// </summary>
public class TaskGate {
    private readonly ConcurrentQueue<ITask> _ProcessesIn = new ();
    private readonly ConcurrentQueue<ITask> _ProcessesOut = new ();
    public readonly AutoResetEvent _AutoResetEvent = new (false);
    public readonly CancellationTokenSource _CancellationTokenSource = new ();
    
    public TaskGate() { 
        var gate = this;
        var cancellationToken = this._CancellationTokenSource.Token;
        Task.Factory.StartNew(() => {
            while (!cancellationToken.IsCancellationRequested) {
                gate._ProcessQueue();
                gate._AutoResetEvent.WaitOne();
            }
        }, cancellationToken);
    }
    
    private void _ProcessQueue() {
        while (this._ProcessesIn.TryDequeue(out var result)) {
            try {
                result.Process();
            }
            catch (Exception e) {
                GD.PrintErr(e);
            }
            this._ProcessesOut.Enqueue(result);
        }
    }
    
    public void Stop() {
        this._CancellationTokenSource.Cancel();
    }
    
    public void Add(ITask process) {
        this._ProcessesIn.Enqueue(process);
        this._AutoResetEvent.Set();
    }
    
    public void Finish() {
        while (this._ProcessesOut.TryDequeue(out var result)) {
            result.Finish();    
        }
    }
}
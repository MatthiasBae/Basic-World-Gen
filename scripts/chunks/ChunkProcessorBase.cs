using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract partial class ChunkProcessorBase : Node {
    public enum ChunkProcessorStep {
        None,
        Prepare,
        Load,
        Render,
        EntitySpawn,
        ActorSpawn,
        ActorDespawn,
        EntityDespawn,
        Unrender,
        Unload,
    }
    
    public enum ChunkProcessState {
        None,
        Prepared,
        Loaded,
        Rendered,
        SpawnedEntities,
        SpawnedActors
    }
    
    public enum ChunkProgressState {
        NotStarted,
        InProgress,
        Completed
    }

    public enum ProcessorUpdateType {
        Downgrade = -1,
        NoChange = 0,
        Upgrade = 1,
    }
    
    [Export]
    public bool UseBatches;
    [Export]
    public int BatchSize;
    
    [Export]
    public ChunkProcessorStep Step;

    [Export]
    public ChunkProcessState ProcessState;
    
    private Queue<Chunk> QueuedChunks = new();
    
    public bool TryStartProcessor(out int chunksLeft) {
        chunksLeft = 0;
        if (this.QueuedChunks.Count == 0) {
            return false;
        }
        
        this._ProcessChunks(out chunksLeft);
        return true;
    }
    
    public void StartProcessor(out int chunksLeft) {
        this._ProcessChunks(out chunksLeft);
    }
    
    public void QueueChunk(Chunk chunk) {
        this.QueuedChunks.Enqueue(chunk);
        chunk.ProcessorStep = this.Step;
        chunk.ProcessState = this.ProcessState;
        chunk.ProgressState = ChunkProgressState.InProgress;
    }
    
    private void _ProcessChunks(out int chunksLeft) {
        chunksLeft = 0;
        if (this.QueuedChunks.Count == 0) {
            return;
        }
        
        var chunksToProcess = this.UseBatches 
            ? this._GetBatch() 
            : this._GetAll();
        
        this.ProcessChunks(chunksToProcess);
        chunksLeft = this.QueuedChunks.Count;
    }
    
    private Dictionary<Vector2I, Chunk> _GetBatch() {
        var chunks = new Dictionary<Vector2I, Chunk>();
        for(var i = 0; i < this.BatchSize; i++) {
            if (this.QueuedChunks.Count == 0) {
                break;
            }
            var chunk = this.QueuedChunks.Dequeue();
            chunks.Add(chunk.Sector, chunk);
        }
        
        return chunks;
    }
    
    private Dictionary<Vector2I, Chunk> _GetAll() {
        var chunks = this.QueuedChunks.ToDictionary(chunk => chunk.Sector);
        this.QueuedChunks.Clear();
        return chunks;
    }
    
    public abstract void ProcessChunks(Dictionary<Vector2I, Chunk> chunks);
    
}

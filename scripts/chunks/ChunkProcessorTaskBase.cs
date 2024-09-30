
using System.Collections.Generic;
using Godot;

public abstract class ChunkProcessorTaskBase : ITask {
    
    public Dictionary<Vector2I, Chunk> Chunks;
    
    public ChunkProcessorTaskBase(Dictionary<Vector2I, Chunk> chunks) {
        this.Chunks = chunks;
    }
    public void Process() {
        foreach(var (sector, chunk) in this.Chunks) {
            this.ProcessChunk(chunk);
        }
    }

    public abstract void Finish();
        
    public abstract void FinishChunk(Chunk chunk);
    
    public abstract void ProcessChunk(Chunk chunk);
}
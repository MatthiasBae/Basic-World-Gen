
using System.Collections.Generic;
using Godot;

public class ChunkProcessorTaskActorSpawn : ChunkProcessorTaskBase {
    
    public ChunkProcessorTaskActorSpawn(Dictionary<Vector2I, Chunk> chunks) : base(chunks) { }
    public override void Finish() {
        foreach(var (sector, chunk) in this.Chunks) {
            this.FinishChunk(chunk);
        }
    }

    public override void FinishChunk(Chunk chunk) {
        chunk.IsThreadLocked = false;
        chunk.ProgressState = ChunkProcessorBase.ChunkProgressState.Completed;
    }

    public override void ProcessChunk(Chunk chunk) {
        
    }
}

using System.Collections.Generic;
using Godot;

public partial class ChunkProcessorEntityDespawn : ChunkProcessorBase {
    
    public override void ProcessChunks(Dictionary<Vector2I, Chunk> chunks) {
        var task = new ChunkProcessorTaskEntityDespawn(chunks);
        task.Process();
        task.Finish();
    }
}
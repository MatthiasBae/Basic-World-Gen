
using System.Collections.Generic;
using Godot;

public partial class ChunkProcessorEntitySpawn : ChunkProcessorBase {
    
    public override void ProcessChunks(Dictionary<Vector2I, Chunk> chunks) {
        var task = new ChunkProcessorTaskEntitySpawn(chunks);
        task.Process();
        task.Finish();
    }
    
}
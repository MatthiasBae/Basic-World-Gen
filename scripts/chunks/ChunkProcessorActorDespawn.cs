
using System.Collections.Generic;
using Godot;

public partial class ChunkProcessorActorDespawn : ChunkProcessorBase {
    public override void ProcessChunks(Dictionary<Vector2I, Chunk> chunks) {
        var task = new ChunkProcessorTaskActorDespawn(chunks);
        task.Process();
        task.Finish();
    }
}

using System.Collections.Generic;
using Godot;

public partial class ChunkProcessorActorSpawn : ChunkProcessorBase{
    public override void ProcessChunks(Dictionary<Vector2I, Chunk> chunks) {
        var task = new ChunkProcessorTaskActorSpawn(chunks);
        task.Process();
        task.Finish();
    }
}
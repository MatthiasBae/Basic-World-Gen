
using System.Collections.Generic;
using Godot;

public partial class ChunkProcessorUnrender : ChunkProcessorBase{
    public override void ProcessChunks(Dictionary<Vector2I, Chunk> chunks) {
        var task = new ChunkProcessorTaskUnrender(chunks);
        task.Process();
        task.Finish();
    }
}
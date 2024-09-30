using Godot;
using System;
using System.Collections.Generic;

public partial class ChunkProcessorPrepare : ChunkProcessorBase {
    public override void ProcessChunks(Dictionary<Vector2I, Chunk> chunks) {
        var task = new ChunkProcessorTaskPrepare(chunks);
        task.Process();
        task.Finish();
    }
}

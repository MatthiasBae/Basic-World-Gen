using Godot;
using System;
using System.Collections.Generic;

public partial class ChunkProcessorUnload : ChunkProcessorBase {
    public override void ProcessChunks(Dictionary<Vector2I, Chunk> chunks) {
        // GD.Print("Unloading chunks");
        var task = new ChunkProcessorTaskUnload(chunks);
        task.Process();
        task.Finish();
    }
}

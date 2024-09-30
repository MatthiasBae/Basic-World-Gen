
using System.Collections.Generic;
using Godot;

public partial class ChunkProcessorRender : ChunkProcessorBase {
    public override void ProcessChunks(Dictionary<Vector2I, Chunk> chunks) {
        TaskHub.Instance.EnterChunkGate(new ChunkProcessorTaskRender(chunks));
    }
}
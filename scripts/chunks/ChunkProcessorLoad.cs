
using System.Collections.Generic;
using Godot;

public partial class ChunkProcessorLoad : ChunkProcessorBase{
    public override void ProcessChunks(Dictionary<Vector2I, Chunk> chunks) {
        TaskHub.Instance.EnterChunkGate(new ChunkProcessorTaskLoad(chunks));
    }
}

using System.Collections.Generic;
using Godot;

public class ChunkProcessorTaskPrepare : ChunkProcessorTaskBase {
    public ChunkProcessorTaskPrepare(Dictionary<Vector2I, Chunk> chunks) : base(chunks) { }
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
        chunk.IsThreadLocked = true;
        chunk.Fields = new Field[ChunkController.CHUNK_SIZE, ChunkController.CHUNK_SIZE];
        for (var x = 0; x < ChunkController.CHUNK_SIZE; x++) {
            for (var y = 0; y < ChunkController.CHUNK_SIZE; y++) {
                
                var localPosition = new Vector2I(x, y);
                var fieldPosition = CoordinatesHelper.LocalPositionToFieldPosition(chunk.Sector, localPosition);
                
                chunk.Fields[x, y] = new Field {
                    LocalPosition = localPosition,
                    FieldPosition = fieldPosition,
                    InChunk = chunk,
                    // Neighbors = new Field[3, 3]
                };
            }
        }
    }
}
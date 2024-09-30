using System.Collections.Generic;
using Godot;

public class ChunkProcessorTaskUnrender : ChunkProcessorTaskBase {
    
    public ChunkProcessorTaskUnrender(Dictionary<Vector2I, Chunk> chunks) : base(chunks) { }
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
        for(var x = 0; x < chunk.Fields.GetLength(0); x++) {
            for(var y = 0; y < chunk.Fields.GetLength(1); y++) {
                var field = chunk.Fields[x, y];
                if(field == null) {
                    continue;
                }
                
                foreach(var (layer, fieldTerrainLayer) in field.Terrain.Layers) {
                    Tilemap.Instance.ClearTile(field.FieldPosition, layer);
                }
            }
        }
    }
}
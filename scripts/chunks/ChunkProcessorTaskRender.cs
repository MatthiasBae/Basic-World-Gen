
using System.Collections.Generic;
using Godot;

public class ChunkProcessorTaskRender : ChunkProcessorTaskBase {
    public Dictionary<Vector2I, List<FieldTerrainRenderData>> TerrainRenderData = [];
    public ChunkProcessorTaskRender(Dictionary<Vector2I, Chunk> chunks) : base(chunks) { }
    public override void Finish() {
        foreach(var (sector, chunk) in this.Chunks) {
            this.FinishChunk(chunk);
        }
    }

    public override void FinishChunk(Chunk chunk) {
        var chunkRenderData = this.TerrainRenderData[chunk.Sector];
        
        foreach(var fieldRenderData in chunkRenderData) {
            Tilemap.Instance.SetTile(fieldRenderData.FieldPosition, fieldRenderData.Layer, fieldRenderData.TileSetSourceId, fieldRenderData.AtlasCoordinate);
        }
        
        chunk.IsThreadLocked = false;
        chunk.ProgressState = ChunkProcessorBase.ChunkProgressState.Completed;
    }

    public override void ProcessChunk(Chunk chunk) {
        chunk.IsThreadLocked = true;
        var chunkRenderData= TerrainRenderer.GetChunkRenderData(chunk);
        
        this.TerrainRenderData.TryAdd(chunk.Sector, chunkRenderData);
    }
}
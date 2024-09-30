
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class ChunkProcessorTaskLoad : ChunkProcessorTaskBase {
    public ChunkProcessorTaskLoad(Dictionary<Vector2I, Chunk> chunks) : base(chunks) { }
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
        var worldConfig = WorldController.Instance.ActiveWorld.Config;
        var biomeConfigs = worldConfig.BiomeConfigs.ToList();
        var abioticFactors = worldConfig.AbioticFactors;
        var abioticFactorNoises = worldConfig.AbioticFactorNoises;

        for(var x = 0; x < chunk.Fields.GetLength(0); x++) {
            for(var y = 0; y < chunk.Fields.GetLength(1); y++) {
                var field = chunk.Fields[x, y];
                // field.Neighbors = ChunkStore.GetSurroundingFields(field.FieldPosition);
                field.Biome = BiomeLocator.GetFieldBiome(field.FieldPosition, abioticFactorNoises, abioticFactors, biomeConfigs);
                field.Terrain = TerrainLocator.GetFieldTerrain(field.FieldPosition, field.Biome.BiomeConfig);
            }
        }
    }
}
using Godot;
using System;
using System.Collections.Generic;

public class TerrainLocator {
    public static FieldTerrainLayer GetFieldTerrainLayer(BiomeConfig biomeConfig, Vector2I fieldPosition, TilemapLayer.Layer layerName) {
        var terrainLayerConfig = biomeConfig.TerrainLayerConfigs[layerName];
        var fieldTerrainLayer = new FieldTerrainLayer() {
            Config = terrainLayerConfig,
            TerrainNoiseValue = terrainLayerConfig.TerrainNoise.GetNoise2Dv(fieldPosition),
        };
        return fieldTerrainLayer;
    }

    public static FieldTerrain GetFieldTerrain(Vector2I fieldPosition, BiomeConfig biomeConfig) {
        var fieldTerrain = new FieldTerrain() {
            Layers = new Dictionary<TilemapLayer.Layer, FieldTerrainLayer>()
        };
        
        foreach(var (layerCategory, terrainLayerConfig) in biomeConfig.TerrainLayerConfigs) {
            var fieldTerrainLayer = GetFieldTerrainLayer(biomeConfig, fieldPosition, layerCategory);
            fieldTerrain.Layers.TryAdd(layerCategory, fieldTerrainLayer);
        }

        foreach(var (layer, fieldTerrainLayer) in fieldTerrain.Layers) {
            fieldTerrainLayer.HasTerrain = layer switch {
                TilemapLayer.Layer.Surface => TerrainRuleSet.HasSurface(fieldTerrain),
                TilemapLayer.Layer.Water => TerrainRuleSet.HasWater(fieldTerrain),
                TilemapLayer.Layer.SubSurface => TerrainRuleSet.HasSubSurface(fieldTerrain),
                TilemapLayer.Layer.Underground => TerrainRuleSet.HasUnderground(fieldTerrain),
                _ => fieldTerrainLayer.HasTerrain
            };
        }
        
        return fieldTerrain;
    }
}


public class TerrainRuleSet {
    public static bool HasWater(FieldTerrain fieldTerrain) {
        return fieldTerrain.HasLayer(TilemapLayer.Layer.Water)
            && fieldTerrain.Layers[TilemapLayer.Layer.Water].Config.Threshold < fieldTerrain.Layers[TilemapLayer.Layer.Water].TerrainNoiseValue;
    }

    public static bool HasSurface(FieldTerrain fieldTerrain) {
        return fieldTerrain.HasLayer(TilemapLayer.Layer.Surface) 
               && !HasWater(fieldTerrain) 
               && fieldTerrain.Layers[TilemapLayer.Layer.Surface].Config.Threshold < fieldTerrain.Layers[TilemapLayer.Layer.Surface].TerrainNoiseValue;
    }
    
    public static bool HasSubSurface(FieldTerrain fieldTerrain) {
        return fieldTerrain.HasLayer(TilemapLayer.Layer.SubSurface)
               && !HasWater(fieldTerrain)
               && fieldTerrain.Layers[TilemapLayer.Layer.SubSurface].Config.Threshold < fieldTerrain.Layers[TilemapLayer.Layer.SubSurface].TerrainNoiseValue;
    }
    
    public static bool HasUnderground(FieldTerrain fieldTerrain) {
        //Is always true, because the underground layer is always present and does not have a threshold
        return true;
    }
}
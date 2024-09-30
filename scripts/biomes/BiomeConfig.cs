using Godot;
using System;

[GlobalClass]
public partial class BiomeConfig : Resource {
    [Export]
    public string Name;
    
    [Export(PropertyHint.MultilineText)]
    public string Description;
    
    [ExportGroup("Biome Generation")]
    [Export]
    public EnvironmentAbioticFactors AbioticFactorValues;
    
    [ExportGroup("Terrain")]
    [Export]
    public Godot.Collections.Dictionary<TilemapLayer.Layer, TerrainLayerConfig> TerrainLayerConfigs;
}

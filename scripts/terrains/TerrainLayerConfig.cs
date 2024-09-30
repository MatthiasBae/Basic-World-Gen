
using Godot;

[GlobalClass]
public partial class TerrainLayerConfig : Resource {
    [Export]
    public int AtlasSourceId;
    
    [ExportGroup("Terrain Generation")]
    [Export(PropertyHint.Range, "-1,1")]
    public float Threshold;
    
    [Export(PropertyHint.File)]
    public FastNoiseLite TerrainNoise;
    
    [ExportGroup("Debugging")]
    [Export]
    public Color Color;
    
}
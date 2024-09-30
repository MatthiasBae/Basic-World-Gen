using Godot;
using System;

[GlobalClass]
public partial class WorldConfig : Resource {
    [Export]
    public string Name;
    
    [Export(PropertyHint.MultilineText)]
    public string Description;
    
    [Export]
    public PackedScene StartingArea;
    
    [ExportGroup("World Generation")]
    [Export]
    public EnvironmentAbioticFactors AbioticFactors;
    [Export]
    public EnvironmentAbioticFactorNoises AbioticFactorNoises;

    [Export]
    public Godot.Collections.Array<BiomeConfig> BiomeConfigs;
}

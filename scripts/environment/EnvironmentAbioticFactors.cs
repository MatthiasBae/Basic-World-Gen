using Godot;
using System;

[GlobalClass]
public partial class EnvironmentAbioticFactors : Resource {
    [Export(PropertyHint.Range, "-1,1")]
    public float Temperature;
    [Export(PropertyHint.Range, "0,2")]
    public float TemperatureWeight = 1;
    
    [Export(PropertyHint.Range, "-1,1")]
    public float Humidity;
    [Export(PropertyHint.Range, "0,2")]
    public float HumidityWeight = 1;
    
    [Export(PropertyHint.Range, "-1,1")]
    public float Elevation;
    [Export(PropertyHint.Range, "0,2")]
    public float ElevationWeight = 1;
}

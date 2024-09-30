using Godot;
using System;

[GlobalClass]
public partial class EnvironmentAbioticFactorNoises : Resource {
    [Export]
    public FastNoiseLite TemperatureNoise;
    
    [Export]
    public FastNoiseLite HumidityNoise;
    
    [Export]
    public FastNoiseLite ElevationNoise;
}

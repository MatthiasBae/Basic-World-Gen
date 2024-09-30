using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class BiomeLocator  {
    /// <summary>
    /// Finds the best matching <see cref="BiomeConfig"/> by checking the <see cref="EnvironmentAbioticFactors"/>
    /// </summary>
    /// <param name="elevation"></param>
    /// <param name="humidity"></param>
    /// <param name="temperature"></param>
    /// <returns></returns>
    public static BiomeConfig GetBiomeConfig(FieldBiomeNoise fieldBiomeNoise, EnvironmentAbioticFactors worldAbioticFactors, List<BiomeConfig> biomeConfigs) {
        var matchingBiomeConfig = (BiomeConfig)null;
        var previousMatchFactor = -100.0;
        foreach(var biomeConfig in biomeConfigs) {
            var biomeConfigAbioticFactors = biomeConfig.AbioticFactorValues;
            
            var matchFactor = Math.Sqrt(
                Math.Pow((fieldBiomeNoise.Elevation - worldAbioticFactors.Elevation) * biomeConfigAbioticFactors.ElevationWeight, 2) +
                Math.Pow((fieldBiomeNoise.Humidity - worldAbioticFactors.Humidity) * biomeConfigAbioticFactors.HumidityWeight, 2) +
                Math.Pow((fieldBiomeNoise.Temperature - worldAbioticFactors.Temperature) * biomeConfigAbioticFactors.TemperatureWeight, 2)
            );
            
            if (!(matchFactor > previousMatchFactor)) {
                continue;
            }

            previousMatchFactor = matchFactor;
            matchingBiomeConfig = biomeConfig;
        }
        
        return matchingBiomeConfig;
    }
    
    
    public static FieldBiome GetFieldBiome(Vector2I fieldPosition, EnvironmentAbioticFactorNoises abioticFactorNoises, EnvironmentAbioticFactors abioticFactors, List<BiomeConfig> biomeConfigs) {
        var fieldBiomeNoise = BiomeLocator.GetFieldBiomeNoise(fieldPosition, abioticFactorNoises);
        
        var biomeConfig = BiomeLocator.GetBiomeConfig(fieldBiomeNoise, abioticFactors, biomeConfigs);
        return new FieldBiome {
            BiomeConfig = biomeConfig,
            Noises = fieldBiomeNoise
        };
    }
    
    public static FieldBiomeNoise GetFieldBiomeNoise(Vector2I fieldPosition, EnvironmentAbioticFactorNoises abioticFactorNoises) {
        return new FieldBiomeNoise {
            Elevation = abioticFactorNoises.ElevationNoise.GetNoise2Dv(fieldPosition),
            Humidity = abioticFactorNoises.HumidityNoise.GetNoise2Dv(fieldPosition),
            Temperature = abioticFactorNoises.TemperatureNoise.GetNoise2Dv(fieldPosition)
        };
    }
}

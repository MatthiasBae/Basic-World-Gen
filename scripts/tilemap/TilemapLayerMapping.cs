using Godot;
using System;

[GlobalClass]
public partial class TilemapLayerMapping : Resource {
    [Export(PropertyHint.Enum)]
    public Godot.Collections.Array<TilemapLayer.Layer> Layers;
}

using Godot;
using System;

[GlobalClass]
public partial class ActorConfig : Resource {

    public enum ActorCreatureType{
        Human,
        Animal,
    }
    
    [Export]
    public string Name;
    
    [Export]
    public ActorCreatureType CreatureType;
}

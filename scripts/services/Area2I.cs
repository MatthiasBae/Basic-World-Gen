
using Godot;

public class Area2I {
    public Vector2I Start; // Top Left
    public Vector2I Center; // Center
    public Vector2I End; // Bottom Right
    public Vector2I Radius; // Half of the size
    
    public Area2I (Vector2I center, Vector2I radius) {
        this.Center = center;
        this.Radius = radius;
        this.Start = center - radius;
        this.End = center + radius;
    }
    
    public bool IsWithinArea(Vector2I position) {
        return position.X >= this.Start.X 
               && position.X <= this.End.X 
               && position.Y >= this.Start.Y 
               && position.Y <= this.End.Y;
    }
}
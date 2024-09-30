using Godot;
using System;

public partial class PlayerFake : CharacterBody2D {
    public override void _Process(double delta) {
        var speed = new Vector2((float)delta, (float)delta);
        if(Input.IsKeyPressed(Key.A)) {
            this.GlobalPosition += new Vector2(-64 * 2, 0) * speed;
        }
        if(Input.IsKeyPressed(Key.D)) {
            this.GlobalPosition += new Vector2(64 * 2, 0) * speed;
        }
        if(Input.IsKeyPressed(Key.W)) {
            this.GlobalPosition += new Vector2(0, -64 * 2) * speed;
        }
        if(Input.IsKeyPressed(Key.S)) {
            this.GlobalPosition += new Vector2(0, 64 * 2) * speed;
        }
        
    }
}

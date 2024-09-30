using Godot;
using System;

public partial class FPSLabel : VBoxContainer {
    [Export]
    public float UpdateInterval = 1f;
    
    [Export]
    private Label _FpsLabel;
    
    [Export]
    private Label _PositionLabel;
    
    [Export]
    private ObjectTracker _ObjectTracker;
    
    private float _TimeSinceLastUpdate = 0;

    public override void _Process(double delta) {
        this._TimeSinceLastUpdate += (float)delta;
        
        this._PositionLabel.Text = $"Position: {CoordinatesHelper.PositionToFieldPosition(this._ObjectTracker.TrackedObject.GlobalPosition)}";
        if(this._TimeSinceLastUpdate >= this.UpdateInterval) {
            this._TimeSinceLastUpdate = 0;
            this._FpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
        }
    }
}

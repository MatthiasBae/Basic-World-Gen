using Godot;
using System;

public partial class ObjectTracker : Node {
    
    [Signal]
    public delegate void SectorEnteredEventHandler(Vector2I sector);
    
    [Signal]
    public delegate void SectorExitedEventHandler(Vector2I sector);
    
    [Signal]
    public delegate void TrackedObjectChangedEventHandler(Node2D trackedObject);
    
    public Vector2 PreviousPosition;
    
    [Export]
    private Node2D _TrackedObject;
    
    public Node2D TrackedObject {
        get => this._TrackedObject;
        set {
            this._TrackedObject = value;
            this._OnTrackedObjectChanged();
            this.EmitSignal(SignalName.TrackedObjectChanged, value);
        }
    }
    
    public Vector2I PreviousSector;
    public Vector2I CurrentSector;
    
    public override void _Ready() {
        if (this.TrackedObject is null) {
            GD.PrintErr("ObjectTracker: TrackedObject is not set.");
            return;
        }
        
        this.SetInitialValues();
    }
    
    public override void _Process(double delta) {
        if (this.TrackedObject is null) {
            return;
        }
        
        this._CheckSectorChanged();
    }

    private void SetInitialValues() {
        this.CurrentSector = CoordinatesHelper.PositionToSector(this.TrackedObject.GlobalPosition);
        this.EmitSignal(SignalName.SectorEntered, this.CurrentSector);
    }

    private void _CheckSectorChanged() {
        if (this.TrackedObject.GlobalPosition == this.PreviousPosition) {
            return;
        }
        
        this._UpdateSectors();
    }
    
    private void _UpdateSectors() {
        this.PreviousPosition = this.TrackedObject.GlobalPosition;
        
        var sector = CoordinatesHelper.PositionToSector(this.TrackedObject.GlobalPosition);
        if (sector == this.CurrentSector) {
            return;
        }
        var oldSector = this.CurrentSector;
        var newSector = sector;
        
        this.PreviousSector = oldSector;
        this.CurrentSector = newSector;
        
        this.EmitSignal(SignalName.SectorEntered, newSector);
        this.EmitSignal(SignalName.SectorExited, oldSector);
    }
    
    
    private void _OnTrackedObjectChanged() {
        this._UpdateSectors();
    }
}

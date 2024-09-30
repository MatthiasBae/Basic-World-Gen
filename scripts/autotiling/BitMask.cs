using Godot;

[Tool]
[GlobalClass]
public partial class BitMask : Resource{
    [ExportGroup("Top row")]
    [Export]
    public int TopLeft;
    [Export]
    public int Top;
    [Export]
    public int TopRight;
    
    [ExportGroup("Middle row")]
    [Export]
    public int Left;
    [Export]
    public int Center;
    [Export]
    public int Right;
    
    [ExportGroup("Bottom row")]
    [Export]
    public int BottomLeft;
    [Export]
    public int Bottom;
    [Export]
    public int BottomRight;
    
    /// <summary>
    /// Returns a 3x3 array representation of the bitmask
    /// </summary>
    /// <returns></returns>
    public int[,] ToArray() {
        return new int[,] {
            { this.TopLeft, this.Top, this.TopRight },
            { this.Left, this.Center, this.Right },
            { this.BottomLeft, this.Bottom, this.BottomRight }
        };
    }
}
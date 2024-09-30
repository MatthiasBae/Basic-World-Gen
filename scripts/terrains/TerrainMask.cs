using Godot;

/// <summary>
/// Represents a 3x3 grid of terrain tiles.
/// Each number is the TileSetSourceId of <see cref="TileSetAtlasSource"/>
/// </summary>
public class TerrainMask : MaskBase {
    public PresenceMask ToPresenceMask() {
        return new PresenceMask {
            TopLeft = this.TopLeft >= 0 ? 1 : 0,
            Top = this.Top >= 0 ? 1 : 0,
            TopRight = this.TopRight >= 0 ? 1 : 0,
            Left = this.Left >= 0 ? 1 : 0,
            Center = this.Center >= 0 ? 1 : 0,
            Right = this.Right >= 0 ? 1 : 0,
            BottomLeft = this.BottomLeft >= 0 ? 1 : 0,
            Bottom = this.Bottom >= 0 ? 1 : 0,
            BottomRight = this.BottomRight >= 0 ? 1 : 0
        };
    }
}

public abstract class MaskBase {
    public int TopLeft;
    public int Top;
    public int TopRight;
    
    public int Left;
    public int Center;
    public int Right;
    
    public int BottomLeft;
    public int Bottom;
    public int BottomRight;
    
    /// <summary>
    /// Returns a 3x3 array representation of the Mask
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
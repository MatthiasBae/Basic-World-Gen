
public class PeeringBitMask : MaskBase {
    public PresenceMask ToPresenceMask() {
        return new PresenceMask {
            BottomLeft = this.BottomLeft == -1 ? 0 : 1,
            Bottom = this.Bottom == -1 ? 0 : 1,
            BottomRight = this.BottomRight == -1 ? 0 : 1,
            TopLeft = this.TopLeft == -1 ? 0 : 1,
            Top = this.Top == -1 ? 0 : 1,
            TopRight = this.TopRight == -1 ? 0 : 1,
            Left = this.Left == -1 ? 0 : 1,
            Right = this.Right == -1 ? 0 : 1,
            Center = this.Center == -1 ? 0 : 1
            
        };
    }
}
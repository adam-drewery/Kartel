namespace Kartel.Console.Controls
{
    public class DoubleStripeBorder : Border
    {
        public override char Top { get; } = '═';
        public override char Left { get; } = '║';
        public override char Bottom { get; } = '═';
        public override char Right { get; } = '║';
        public override char TopLeft { get; } = '╔';
        public override char TopRight { get; } = '╗';
        public override char BottomLeft { get; } = '╚';
        public override char BottomRight { get; } = '╝';
        public override char LeftT { get; } = '╠';
        public override char RightT { get; } = '╣';
    }
    
    public class ThickBorder : Border
    {
        public override char Top { get; } = '━';
        public override char Left { get; } = '┃';
        public override char Bottom { get; } = '━';
        public override char Right { get; } = '┃';
        public override char TopLeft { get; } = '┏';
        public override char TopRight { get; } = '┓';
        public override char BottomLeft { get; } = '┗';
        public override char BottomRight { get; } = '┛';
        public override char LeftT { get; } = '┣';
        public override char RightT { get; } = '┫';
    }

    public abstract class Border
    {
        public abstract char Top { get; }
        
        public abstract char Left { get; }
        
        public abstract char Bottom { get; }
        
        public abstract char Right { get; }
        
        public abstract char TopLeft { get; }
        
        public abstract char TopRight { get; }
        
        public abstract char BottomLeft { get; }
        
        public abstract char BottomRight { get; }
        
        public abstract char LeftT { get; }
        
        public abstract char RightT { get; }
    }
}
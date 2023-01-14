namespace Kartel.Console.Controls
{
    public abstract class Control
    {
        public abstract void Update();

        public char ShortcutKey { get; set; }
        
        public abstract int Height { get; }
        
        public abstract int Width { get; }
     
        public abstract int? X { get; }
        
        public abstract int? Y { get; }
        
        public abstract void Render(int width);
    }
}
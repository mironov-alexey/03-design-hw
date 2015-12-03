using System.Drawing;
using Point = Microsoft.Xna.Framework.Point;
namespace _03_design_hw
{
    public class Tag
    {
        public Tag(Word word, Point location, Font font, Color color)
        {
            Word = word;
            Location = location;
            Color = color;
            Font = font;
        }

        public Word Word{ get; }
        public Point Location{ get; }
        public Color Color{ get; }
        public Font Font{ get; }
    }
}

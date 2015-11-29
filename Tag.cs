using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Microsoft.Xna.Framework.Point;
namespace _03_design_hw
{
    public class Tag
    {
        public Tag(Word word, Point location, SizeF size, Font font, Color color)
        {
            Word = word;
            Location = location;
            Size = size;
            Color = color;
            Font = font;
        }

        public Word Word{ get; }
        public Point Location{ get; }
        public SizeF Size{ get; }
        public Color Color{ get; }
        public Font Font{ get; }
    }
}

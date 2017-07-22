using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HundredsGame
{
    class Border
    {
        public Rectangle Rect { get; set; }

        public Border(int x, int y, int width, int height)
        {
            Rect = new Rectangle(x, y, width, height);
        }

    }
}

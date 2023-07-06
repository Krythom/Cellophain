using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cellophain
{
    class Instruction
    {
        private Point coords;
        private Element element;

        public Instruction(int x, int y, Element element)
        {
            this.coords = new Point(x, y);
            this.element = element;
        }

        public Element GetElement()
        {
            return element;
        }

        public Point GetCoords()
        {
            return coords;
        }
    }
}

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
        private string type;
        private string key;
        private Object val;

        public Instruction(int x, int y, Element element)
        {
            this.coords = new Point(x, y);
            this.element = element;
            type = "move";
        }

        public Instruction(Element element, string key, Object val)
        {
            this.element = element;
            this.key = key;
            this.val = val;
            type = "value";
        }

        public string InstructionType()
        {
            return type;
        }

        public Element GetElement()
        {
            return element;
        }

        public Point GetCoords()
        {
            return coords;
        }

        public Object GetVal()
        {
            return val;
        }

        public string GetKey()
        {
            return key;
        }
    }
}

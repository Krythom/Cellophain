using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cellophain
{
    abstract class Element
    {
        protected string name;
        protected int r;
        protected int g;
        protected int b;

        public abstract Request Iterate(Element[,] world, int xPos, int yPos);

        public string GetName()
        {
            return name;
        }

        public Color GetColor()
        {
            return new Color(r, g, b);
        }

        public Element CheckCell(Element[,] world, int x, int y)
        {
            if (x >= 0 && y >= 0 && x < world.GetLength(0) && y < world.GetLength(0))
            {
                return world[x, y];
            }
            else
            {
                return new Boundary();
            }
        }
    }
}

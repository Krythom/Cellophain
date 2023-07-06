using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Boundary : Element
    {
        public Boundary()
        {
            name = "boundary";
            r = 0;
            g = 0;
            b = 0;
        }

        public override Request Iterate(Element[,] world, int xPos, int yPos)
        {
            return null;
        }
    }
}

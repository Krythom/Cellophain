using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Void : Element
    { 
        public Void()
        {
            name = "void";
            r = 10;
            g = 10;
            b = 20;
        }

        public override Request Iterate(Element[,] world, int xPos, int yPos)
        {
            return null;
        }
    }
}

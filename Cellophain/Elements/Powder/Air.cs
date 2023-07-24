using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    class Air : Powder
    {
        public Air()
        {
            name = "air";
            r = 10;
            g = 10;
            b = 20;
            temp = 15;
            density = 0.001225;
            matterType = "gas";
        }

        public override Request Iterate(Element[,] world, int xPos, int yPos)
        {
            return null;
        }
    }
}

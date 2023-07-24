using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    //This cell type only exists to be returned when a cell would be checking an index outside the bounds of the world
    //Just a way of avoiding bounds checking for some slightly nicer looking code 

    class Boundary : Powder
    {
        public Boundary()
        {
            name = "boundary";
            r = 0;
            g = 0;
            b = 0;
            matterType = "solid";
            density = double.MaxValue;
            temp = 15;
        }

        public override Request Iterate(Element[,] world, int xPos, int yPos)
        {
            return null;
        }
    }
}

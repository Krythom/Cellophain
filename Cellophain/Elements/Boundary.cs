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
            vars["name"] = "boundary";
            vars["r"] = 0;
            vars["g"] = 0;
            vars["b"] = 0;
            vars["matterType"] = "solid";
            vars["density"] = double.MaxValue;
            vars["temp"] = 0;
            vars["heatCapacity"] = 0;
        }

        public override Request Iterate(Element[,] world)
        {
            return null;
        }
    }
}

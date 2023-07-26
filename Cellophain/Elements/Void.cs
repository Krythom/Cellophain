using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Void : Element
    { 
        public Void()
        {
            vars["name"] = "void";
            vars["r"] = 10;
            vars["g"] = 10;
            vars["b"] = 20;
        }

        public override Request Iterate(Element[,] world)
        {
            return null;
        }
    }
}

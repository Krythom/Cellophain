using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Water : Powder
    {
        public Water()
        {
            vars["name"] = "water";
            vars["r"] = 100;
            vars["g"] = 186;
            vars["b"] = 235;
            vars["matterType"] = "liquid";
            vars["density"] = 1;
            vars["temp"] = 15;
            vars["heatCapacity"] = 4.186;
        }

        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new List<Instruction>();
            instructions = FluidUpdate(world, this, instructions);
            instructions.Add(new Instruction(this, "temp", this.GetTemp() + TempChange(world, this)));
            return new Request(instructions);
        }
    }
}

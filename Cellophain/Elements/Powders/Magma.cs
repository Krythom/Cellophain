using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    class Magma : Powder
    {
        public Magma()
        {
            vars["name"] = "magma";
            vars["r"] = 170;
            vars["g"] = 40;
            vars["b"] = 15;
            vars["matterType"] = "liquid";
            vars["density"] = 3;
            vars["temp"] = 1500;
            vars["heatCapacity"] = 1.45;
            vars["speed"] = 0.1;
        }

        public override Request Iterate(Element[,] world)
        {
            List <Instruction> instructions = new();
            instructions = FluidUpdate(world, this, instructions);
            instructions.Add(new Instruction(this, "temp", GetTemp() + TempChange(world, this)));
            return new Request(instructions);
        }
    }
}

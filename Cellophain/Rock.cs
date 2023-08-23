using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    internal class Rock : Powder
    {
        public Rock()
        {
            vars["name"] = "rock";
            vars["r"] = 75;
            vars["g"] = 75;
            vars["b"] = 75;
            vars["matterType"] = "solid";
            vars["temp"] = 15;
            vars["density"] = 2.71;
            vars["heatCapacity"] = 0.91;
            vars["speed"] = 1;
            vars["set"] = false;
        }

        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new();
            instructions = SolidUpdate(world, this, instructions);
            instructions.Add(new Instruction(this, "temp", this.GetTemp() + TempChange(world, this)));
            return new Request(instructions);
        }
    }
}

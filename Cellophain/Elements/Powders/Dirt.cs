using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    internal class Dirt : Powder
    {
        public Dirt()
        {
            vars["name"] = "dirt";
            vars["r"] = 75;
            vars["g"] = 50;
            vars["b"] = 25;
            vars["matterType"] = "powder";
            vars["temp"] = 15;
            vars["density"] = 1.76;
            vars["heatCapacity"] = 1.3;
            vars["speed"] = 0.3;
        }

        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new();
            instructions = PowderUpdate(world, this, instructions);
            instructions.Add(new Instruction(this, "temp", this.GetTemp() + TempChange(world, this)));
            return new Request(instructions);
        }
    }
}

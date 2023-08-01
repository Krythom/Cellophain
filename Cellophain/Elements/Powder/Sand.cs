using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Sand : Powder
    {
        public Sand()
        {
            vars["name"] = "sand";
            vars["r"] = 235;
            vars["g"] = 180;
            vars["b"] = 120;
            vars["matterType"] = "powder";
            vars["temp"] = 15;
            vars["density"] = 1.52;
            vars["heatCapacity"] = 0.290;
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

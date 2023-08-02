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
            vars["speed"] = 1;
        }

        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new List<Instruction>();
            instructions = FluidUpdate(world, this, instructions);
            instructions.Add(new Instruction(this, "temp", this.GetTemp() + TempChange(world, this)));

            if (GetTemp() > 100)
            {
                instructions.Clear();
                Steam toPlace = new();
                instructions.Add(new Instruction(GetLocation().X, GetLocation().Y, toPlace));
                instructions.Add(new Instruction(toPlace, "temp", GetTemp()));
            }

            return new Request(instructions);
        }
    }
}

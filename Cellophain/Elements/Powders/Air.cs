﻿using System;
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
            vars["name"] = "air";
            vars["r"] = 10;
            vars["g"] = 10;
            vars["b"] = 20;
            vars["temp"] = 15;
            vars["density"] = 0.001225;
            vars["matterType"] = "gas";
            vars["heatCapacity"] = 1;
            vars["speed"] = 1;
        }

        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new();
            instructions = FluidUpdate(world, this, instructions);
            instructions.Add(new Instruction(this, "temp", this.GetTemp() + TempChange(world, this)));
            //instructions.Add(new Instruction(this, "r", GetTemp()));
            return new Request(instructions);
        }
    }
}

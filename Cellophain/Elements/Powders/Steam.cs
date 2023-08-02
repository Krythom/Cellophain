using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    class Steam : Powder
    {
        Random rand = new();
        public Steam()
        {
            vars["name"] = "steam";
            vars["r"] = 55;
            vars["g"] = 98;
            vars["b"] = 123;
            vars["temp"] = 200;
            vars["density"] = 0.000598;
            vars["matterType"] = "gas";
            vars["heatCapacity"] = 2.03;
            vars["speed"] = 1;
        }

        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new();
            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;
            Powder left = (Powder)CheckCell(world, xPos - 1, yPos);
            Powder right = (Powder)CheckCell(world, xPos + 1, yPos);

            instructions = FluidUpdate(world, this, instructions);
            instructions.Add(new Instruction(this, "temp", this.GetTemp() + TempChange(world, this)));

            if ((GetTemp() < 100 && left.GetName() == "steam" &&  right.GetName() == "steam") || GetTemp() < 75)
            {
                Mist toPlace = new();
                instructions.Add(new Instruction(xPos, yPos, toPlace));
                instructions.Add(new Instruction(toPlace, "temp", GetTemp()));
            }

            return new Request(instructions);
        }
    }
}

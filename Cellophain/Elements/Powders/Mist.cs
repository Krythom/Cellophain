using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    internal class Mist : Powder
    {
        public Mist()
        {
            vars["name"] = "mist";
            vars["r"] = 10;
            vars["g"] = 10;
            vars["b"] = 20;
            vars["matterType"] = "gas";
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
            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;
            Powder left = (Powder)CheckCell(world, xPos - 1, yPos);
            Powder right = (Powder)CheckCell(world, xPos + 1, yPos);
            Powder up = (Powder)CheckCell(world, xPos, yPos - 1);
            Powder down = (Powder)CheckCell(world,xPos, yPos + 1);

            if (GetTemp() > 100)
            {
                instructions.Clear();
                Steam toPlace = new();
                instructions.Add(new Instruction(GetLocation().X, GetLocation().Y, toPlace));
                instructions.Add(new Instruction(toPlace, "temp", GetTemp()));
            }

            if (left.GetMatter() != "gas" || up.GetMatter() != "gas" ||  down.GetMatter() != "gas" || right.GetMatter() != "gas")
            {
                instructions.Clear();
                Water toPlace = new();
                instructions.Add(new Instruction(GetLocation().X, GetLocation().Y, toPlace));
                instructions.Add(new Instruction(toPlace, "temp", GetTemp()));
            }

            return new Request(instructions);
        }
    }
}

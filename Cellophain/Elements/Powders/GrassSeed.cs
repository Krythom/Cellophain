using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cellophain
{
    internal class GrassSeed : Powder
    {
        Random rand = new();
        public GrassSeed()
        {
            vars["name"] = "grassSeed";
            vars["r"] = 150;
            vars["g"] = 100;
            vars["b"] = 100;
            vars["matterType"] = "powder";
            vars["temp"] = 15;
            vars["density"] = 1.2;
            vars["heatCapacity"] = 0.2;
            vars["speed"] = 1;
        }

        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new();
            instructions = PowderUpdate(world, this, instructions);
            instructions.Add(new Instruction(this, "temp", this.GetTemp() + TempChange(world, this)));
            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;
            Powder down = (Powder)CheckCell(world, xPos, yPos + 1);

            if (!(down.GetMatter() is "gas" or "liquid"))
            {
                instructions.Add(new Instruction(xPos, yPos, new Air()));
            }
            if (down.GetName() == "dirt")
            {
                Grass toPlace = new();
                instructions.Add(new Instruction(xPos, yPos, toPlace));
                instructions.Add(new Instruction(toPlace, "growth", rand.Next(4)));
            }

            return new Request(instructions);
        }
    }
}

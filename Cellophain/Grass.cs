using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    internal class Grass : Powder
    {
        Random rand = new();

        public Grass()
        {
            vars["name"] = "grass";
            vars["r"] = 110;
            vars["g"] = 145;
            vars["b"] = 75;
            vars["matterType"] = "plant";
            vars["temp"] = 15;
            vars["density"] = 1.52;
            vars["heatCapacity"] = 0.290;
            vars["growth"] = 1;
        }

        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new();
            instructions.Add(new Instruction(this, "temp", this.GetTemp() + TempChange(world, this)));
            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;
            Powder up = (Powder)CheckCell(world, xPos, yPos - 1);
            Powder down = (Powder)CheckCell(world, xPos, yPos + 1);

            if (GetGrowth() > 0 && up.GetMatter() is "liquid" or "gas")
            {
                Grass toPlace = new Grass();
                instructions.Add(new Instruction(xPos, yPos - 1, toPlace));
                instructions.Add(new Instruction(toPlace, "growth", GetGrowth() - 1));
                instructions.Add(new Instruction(toPlace, "g", GetColor().G - 15));
                instructions.Add(new Instruction(this, "growth", 0));
            }

            if (!(down.GetName() is "dirt" or "grass"))
            {
                instructions.Clear();
                if (rand.NextDouble() < 0.2)
                {
                    instructions.Add(new Instruction(xPos, yPos, new GrassSeed()));
                }
                else
                {
                    instructions.Add(new Instruction(xPos, yPos, new Air()));
                }
            }

            return new Request(instructions);
        }

        public int GetGrowth()
        {
            return System.Convert.ToInt32(vars["growth"]);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Sand : Powder
    {
        Random rand = new Random();
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
            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;
            List<Instruction> instructions = new List<Instruction>();

            Powder down = (Powder) CheckCell(world, xPos, yPos + 1);
            Powder downLeft = (Powder) CheckCell(world, xPos - 1, yPos + 1);
            Powder downRight = (Powder) CheckCell(world, xPos + 1, yPos + 1);

            instructions.Add(new Instruction(this, "temp", this.GetTemp() + TempChange(world, this, xPos, yPos)));

            if (down.GetMatter() is "gas" or "liquid")
            {
                if (rand.NextDouble() > down.GetDensity() / this.GetDensity())
                {
                    instructions.Add(new Instruction(xPos, yPos, down));
                    instructions.Add(new Instruction(xPos, yPos + 1, this));
                }
            }
            else
            {
                //Randomize order of directional checks to avoid left bias
                Powder[] sides = {downLeft, downRight};
                int first = rand.Next(2);

                if (sides[first].GetMatter() is "gas" or "liquid")
                {
                    if (rand.NextDouble() > sides[first].GetDensity() / this.GetDensity())
                    {
                        instructions.Add(new Instruction(xPos, yPos, sides[first]));
                        instructions.Add(new Instruction(xPos + (2 * first) - 1, yPos + 1, this));
                    }
                }
                else if (sides[(first + 1) % 2].GetMatter() is "gas" or "liquid")
                {
                    if (rand.NextDouble() > sides[first].GetDensity() / this.GetDensity())
                    {
                        instructions.Add(new Instruction(xPos, yPos, sides[(first + 1) % 2]));
                        instructions.Add(new Instruction(xPos + (2 * ((first + 1) % 2)) - 1, yPos + 1, this));
                    }
                }
            }

            return new Request(instructions);
        }
    }
}

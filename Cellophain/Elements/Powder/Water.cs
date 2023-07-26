using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Water : Powder
    {
        Random rand = new Random();
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
        }

        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new List<Instruction>();

            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;

            Powder left = (Powder) CheckCell(world, xPos - 1, yPos);
            Powder right = (Powder) CheckCell(world, xPos + 1, yPos);
            Powder down = (Powder) CheckCell(world, xPos, yPos + 1);

            instructions.Add(new Instruction(this, "temp", this.GetTemp() + TempChange(world, this, xPos, yPos)));
            instructions.Add(new Instruction(this, "r", vars["temp"]));

            //Movement
            if (down.GetMatter() is "gas" or "liquid" && down.GetName() != (string) vars["name"])
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
                Powder[] sides = {left, right};
                int first = rand.Next(2);

                if (sides[first].GetMatter() is "gas" or "liquid" && sides[first].GetName() != (string) vars["name"])
                {
                    if ((rand.NextDouble() > sides[first].GetDensity() / this.GetDensity()))
                    {
                        instructions.Add(new Instruction(xPos, yPos, sides[first]));
                        instructions.Add(new Instruction(xPos + (2 * first) - 1, yPos, this));
                    }
                }
                else if (sides[(first + 1) % 2].GetMatter() is "gas" or "liquid" && sides[(first + 1) % 2].GetName() != (string) vars["name"])
                {
                    if (rand.NextDouble() > sides[(first + 1) % 2].GetDensity() / this.GetDensity())
                    {
                        instructions.Add(new Instruction(xPos, yPos, sides[(first + 1) % 2]));
                        instructions.Add(new Instruction(xPos + (2 * ((first + 1) % 2)) - 1, yPos, this));
                    }
                }
            }

            //Boiling
            if (GetTemp() >= 100)
            {
                instructions.Clear();
                instructions.Add(new Instruction(xPos, yPos, new Air()));
            }

            return new Request(instructions);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Water : Powder
    {
        Dictionary<string, Powder> cells = new Dictionary<string, Powder>();
        Random rand = new Random();
        public Water()
        {
            name = "water";
            r = 100;
            g = 186;
            b = 235;
            matterType = "liquid";
            density = 1;
            temp = 15;
        }

        public override Request Iterate(Element[,] world, int xPos, int yPos)
        {
            List<Instruction> instructions = new List<Instruction>();

            cells["left"] = CheckPowder(world, xPos - 1, yPos);
            cells["right"] = CheckPowder(world, xPos + 1, yPos);
            cells["down"] = CheckPowder(world, xPos, yPos + 1);

            if (cells["down"].GetMatter() is "gas")
            {
                instructions.Add(new Instruction(xPos, yPos, cells["down"]));
                instructions.Add(new Instruction(xPos, yPos + 1, new Water()));
            }
            else
            {
                Powder[] sides = {cells["left"], cells["right"]};
                int first = rand.Next(2);

                if (sides[first].GetMatter() is "gas")
                {
                    instructions.Add(new Instruction(xPos, yPos, sides[first]));
                    instructions.Add(new Instruction(xPos + (2 * first) - 1, yPos, new Water()));
                }
                else if (sides[(first + 1) % 2].GetMatter() is "gas")
                {
                    instructions.Add(new Instruction(xPos, yPos, sides[(first + 1) % 2]));
                    instructions.Add(new Instruction(xPos + (2 * ((first + 1) % 2)) - 1, yPos, new Water()));
                }
            }

            return new Request(instructions);
        }
    }
}

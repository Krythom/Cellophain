using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Sand : Powder
    {
        Dictionary<string, Powder> cells = new Dictionary<string, Powder>();
        Random rand = new Random();
        public Sand()
        {
            name = "sand";
            r = 220;
            g = 190;
            b = 120;
            matterType = "powder";
            temp = 15;
            density = 1.52;
        }

        //Currently has bias due to how it processes right then left, if redoing this element fix it to be like water
        //Also if coming back to this to add more powder elements probably add a way to make falling/sliding/etc into different elements generic rather than adding new rules for each one

        public override Request Iterate(Element[,] world, int xPos, int yPos)
        {
            List<Instruction> instructions = new List<Instruction>();

            cells["down"] = CheckPowder(world, xPos, yPos + 1);
            cells["downleft"] = CheckPowder(world, xPos - 1, yPos + 1);
            cells["downright"] = CheckPowder(world, xPos + 1, yPos + 1);

            if (cells["down"].GetMatter() is "gas" or "liquid")
            {
                if (rand.NextDouble() > cells["down"].GetDensity() / density)
                {
                    instructions.Add(new Instruction(xPos, yPos, cells["down"]));
                    instructions.Add(new Instruction(xPos, yPos + 1, new Sand()));
                }
            }
            else
            {
                Powder[] sides = { cells["downleft"], cells["downright"] };
                int first = rand.Next(2);

                if (sides[first].GetMatter() is "gas" or "liquid")
                {
                    if (rand.NextDouble() > sides[first].GetDensity() / density)
                    {
                        instructions.Add(new Instruction(xPos, yPos, sides[first]));
                        instructions.Add(new Instruction(xPos + (2 * first) - 1, yPos + 1, new Sand()));
                    }
                }
                else if (sides[(first + 1) % 2].GetMatter() is "gas" or "liquid")
                {
                    if (rand.NextDouble() > sides[first].GetDensity() / density)
                    {
                        instructions.Add(new Instruction(xPos, yPos, sides[(first + 1) % 2]));
                        instructions.Add(new Instruction(xPos + (2 * ((first + 1) % 2)) - 1, yPos + 1, new Sand()));
                    }
                }
            }

            return new Request(instructions);
        }
    }
}

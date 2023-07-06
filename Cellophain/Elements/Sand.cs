using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Sand : Element
    {
        Random rand = new Random();
        public Sand()
        {
            name = "sand";
            r = 220;
            g = 190;
            b = 120;
        }

        //Currently has bias due to how it processes right then left, if redoing this element fix it to be like water
        //Also if coming back to this to add more powder elements probably add a way to make falling/sliding/etc into different elements generic rather than adding new rules for each one

        public override Request Iterate(Element[,] world, int xPos, int yPos)
        {
            List<Instruction> instructions = new List<Instruction>();

            if (CheckCell(world, xPos, yPos + 1).GetName() == "void")
            {
                instructions.Add(new Instruction(xPos, yPos, new Void()));
                instructions.Add(new Instruction(xPos, yPos + 1, new Sand()));
            }
            else if (CheckCell(world, xPos + 1, yPos + 1).GetName().Equals("void") && rand.NextDouble() < 0.5)
            {
                instructions.Add(new Instruction(xPos, yPos, new Void()));
                instructions.Add(new Instruction(xPos + 1, yPos + 1, new Sand()));
            }
            else if (CheckCell(world, xPos - 1, yPos + 1).GetName().Equals("void"))
            {
                instructions.Add(new Instruction(xPos, yPos, new Void()));
                instructions.Add(new Instruction(xPos - 1, yPos + 1, new Sand()));
            }

            if (rand.NextDouble() < 0.2)
            {
                if (CheckCell(world, xPos, yPos + 1).GetName() == "water")
                {
                    instructions.Add(new Instruction(xPos, yPos, new Water()));
                    instructions.Add(new Instruction(xPos, yPos + 1, new Sand()));
                }
            }

            if (rand.NextDouble() < 0.05)
            {
                if (CheckCell(world, xPos + 1, yPos + 1).GetName() == "water" && rand.NextDouble() < 0.5)
                {
                    instructions.Add(new Instruction(xPos, yPos, new Water()));
                    instructions.Add(new Instruction(xPos + 1, yPos + 1, new Sand()));
                }
                else if (CheckCell(world, xPos - 1, yPos + 1).GetName() == "water")
                {
                    instructions.Add(new Instruction(xPos, yPos, new Water()));
                    instructions.Add(new Instruction(xPos - 1, yPos + 1, new Sand()));
                }
            }

            return new Request(instructions);
        }
    }
}

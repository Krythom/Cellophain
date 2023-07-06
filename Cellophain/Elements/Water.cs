using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Water : Element
    {
        Random rand = new Random();
        public Water()
        {
            name = "water";
            r = 100;
            g = 186;
            b = 235;
        }

        public override Request Iterate(Element[,] world, int xPos, int yPos)
        {
            List<Instruction> instructions = new List<Instruction>();

            if (CheckCell(world, xPos, yPos + 1).GetName() == "void")
            {
                instructions.Add(new Instruction(xPos, yPos, new Void()));
                instructions.Add(new Instruction(xPos, yPos + 1, new Water()));
            }
            else if(CheckCell(world, xPos + 1, yPos).GetName() == "void" && CheckCell(world, xPos - 1, yPos).GetName() == "void")
            {
                if (rand.NextDouble() < 0.5)
                {
                    instructions.Add(new Instruction(xPos, yPos, new Void()));
                    instructions.Add(new Instruction(xPos + 1, yPos, new Water()));
                }
                else
                {
                    instructions.Add(new Instruction(xPos, yPos, new Void()));
                    instructions.Add(new Instruction(xPos - 1, yPos, new Water()));
                }
            }
            else if (CheckCell(world, xPos + 1, yPos).GetName() == "void")
            {
                instructions.Add(new Instruction(xPos, yPos, new Void()));
                instructions.Add(new Instruction(xPos + 1, yPos, new Water()));
            }
            else if (CheckCell(world, xPos - 1, yPos).GetName() == "void")
            {
                instructions.Add(new Instruction(xPos, yPos, new Void()));
                instructions.Add(new Instruction(xPos - 1, yPos, new Water()));
            }

            return new Request(instructions);
        }
    }
}

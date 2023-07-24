using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    class HueGene : Element
    {
        Random rand = new Random();

        public HueGene(int red, int green, int blue) 
        {
            name = "hueGene";
            r = red;
            g = green;
            b = blue;
        }

        public override Request Iterate(Element[,] world, int xPos, int yPos)
        {
            List<Instruction> instructions = new List<Instruction>();
            int mutationStrength = 10;
            int dir = rand.Next(4);
            int x;
            int y;

            if (dir == 0)
            {
                x = 0;
                y = 1;
            }
            else if (dir == 1)
            {
                x = 1;
                y = 0;
            }
            else if (dir == 2)
            {
                x = 0;
                y = -1;
            }
            else
            {
                x = -1;
                y = 0;
            }

            if (CheckCell(world, xPos + x, yPos + y).GetName() == "void")
            {
                instructions.Add(new Instruction(xPos + x, yPos + y, new HueGene(r + rand.Next(-1 * mutationStrength, mutationStrength + 1), g + rand.Next(-1 * mutationStrength, mutationStrength + 1), b + rand.Next(-1 * mutationStrength, mutationStrength + 1))));
            }

            return new Request(instructions);
        }
    }
}

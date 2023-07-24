using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Sheep : Element
    {
        private bool alive;
        public Sheep(bool alive)
        {
            name = "sheep";
            if (alive)
            {
                r = 256;
                g = 256;
                b = 256;
            }
            else
            {
                r = 0;
                g = 0;
                b = 0;
            }

            this.alive = alive;
        }
        public override Request Iterate(Element[,] world, int xPos, int yPos)
        {
            List<Instruction> instructions = new List<Instruction>();

            if (GetNeighbors(world, xPos, yPos) >= 4)
            {
                instructions.Add(new Instruction(xPos, yPos, new Sheep(true)));
            }
            else
            {
                instructions.Add(new Instruction(xPos, yPos, new Sheep(false)));
            }

            return new Request(instructions);
        }

        private int GetNeighbors(Element[,] world, int xPos, int yPos)
        {
            int neighbors = 0;
            for (int x = xPos - 1; x <= xPos + 1; x++)
            {
                for (int y = yPos - 1; y <= yPos + 1; y++)
                {
                    Sheep toCheck = (Sheep)world[Mod(x, world.GetLength(0)), Mod(y, world.GetLength(0))];
                    if (!(y == yPos && x == xPos) && toCheck.IsAlive())
                    {
                        neighbors++;
                    }
                }
            }

            return neighbors;
        }

        public bool IsAlive()
        {
            return alive;
        }

        private int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}

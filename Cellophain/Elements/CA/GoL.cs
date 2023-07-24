using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class GoL : Element
    {
        private bool alive;
        public GoL(bool alive)
        {
            name = "GoL";
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

            if (alive)
            {
                if (GetNeighbors(world, xPos, yPos) != 2 && GetNeighbors(world, xPos, yPos) != 3)
                {
                    instructions.Add(new Instruction(xPos, yPos, new GoL(false)));
                }
            }
            else if (GetNeighbors(world, xPos, yPos) == 3)
            {
                instructions.Add(new Instruction(xPos, yPos, new GoL(true)));
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
                    GoL toCheck = (GoL) world[Mod(x, world.GetLength(0)), Mod(y, world.GetLength(0))];
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

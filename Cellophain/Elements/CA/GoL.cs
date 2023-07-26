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
            vars["name"] = "GoL";
            if (alive)
            {
                vars["r"] = 256;
                vars["g"] = 256;
                vars["b"] = 256;
            }
            else
            {
                vars["r"] = 0;
                vars["g"] = 0;
                vars["b"] = 0;
            }

            this.alive = alive;
        }
        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new List<Instruction>();

            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;

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

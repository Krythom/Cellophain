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
            vars["name"] = "sheep";
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
            int xPos = this.GetLocation().X;
            int yPos = this.GetLocation().Y;

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

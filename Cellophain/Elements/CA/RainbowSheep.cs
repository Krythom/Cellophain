using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class RainbowSheep : Element
    {
        int id;
        int n;
        Random rand = new Random();
        public RainbowSheep(int red, int green, int blue, int id, int n)
        {
            this.id = id;
            this.n = n;
            vars["name"] = "rainbowSheep";
            vars["r"] = red;
            vars["g"] = green;
            vars["b"] = blue;
        }
        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new List<Instruction>();
            int xPos = this.GetLocation().X;
            int yPos = this.GetLocation().Y;
            RainbowSheep winner = GetWinner(world, xPos, yPos);

            if (winner != null)
            {
                instructions.Add(new Instruction(xPos, yPos, winner));
            }
            else
            {
                instructions.Add(new Instruction(xPos, yPos, this));
            }

            return new Request(instructions);
        }

        private RainbowSheep GetWinner(Element[,] world, int xPos, int yPos)
        {
            RainbowSheep[] elements = new RainbowSheep[n];
            int[] neighbors = new int[n];

            for (int x = xPos - 1; x <= xPos + 1; x++)
            {
                for (int y = yPos - 1; y <= yPos + 1; y++)
                {
                   if (CheckCell(world, x, y).GetName() == "rainbowSheep")
                   {
                        RainbowSheep toCheck = (RainbowSheep) CheckCell(world, x, y);
                        if (!(y == yPos && x == xPos))
                        {
                            elements[toCheck.id] = toCheck;
                            neighbors[toCheck.id]++;
                        }
                   }
                }
            }

            int winningId = -1;
            int highestCount = -1;

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] > highestCount)
                {
                    winningId = i;
                    highestCount = neighbors[i];
                }
                if (neighbors[i] == highestCount && rand.NextDouble() < 0.5)
                {
                    winningId = i;
                    highestCount = neighbors[i];
                }
            }

            return elements[winningId];
        }

        private int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}

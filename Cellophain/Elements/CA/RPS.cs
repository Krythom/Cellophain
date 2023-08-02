using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    class RPS : Element
    {
        int id;
        int n;
        static int[,] winners;
        Random rand = new Random();

        public RPS(int red, int green, int blue, int id, int n)
        {
            this.id = id;
            this.n = n;
            vars["name"] = "RPS";
            vars["r"] = red;
            vars["g"] = green;
            vars["b"] = blue;
        }

        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new List<Instruction>();
            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;
            RPS winner = GetWinner(world, xPos, yPos);

            if (winner != null)
            {
                instructions.Add(new Instruction(xPos, yPos, (Element)winner.DeepCopy()));
            }
            else
            {
                instructions.Add(new Instruction(xPos, yPos, this));
            }

            return new Request(instructions);
        }


        private RPS GetWinner(Element[,] world, int xPos, int yPos)
        {
            RPS[] elements = new RPS[n];
            int[] neighbors = new int[n];

            for (int x = xPos - 1; x <= xPos + 1; x++)
            {
                for (int y = yPos - 1; y <= yPos + 1; y++)
                {
                    RPS toCheck = (RPS) CheckCell(world, Mod(x, world.GetLength(0)), Mod(y, world.GetLength(0)));
                    if (!(y == yPos && x == xPos))
                    {
                        elements[toCheck.id] = toCheck;
                        neighbors[toCheck.id]++;
                    }
                }
            }

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] > 2 && i != id)
                {
                    if (winners[elements[i].id, id] == elements[i].id)
                    {
                        return elements[i];
                    }
                }
            }

            return null;
        }

        public static void CalculateWinners(int n)
        {
            winners = new int[n, n];

            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    int dif = Math.Abs(x - y);

                    if ( dif > Math.Floor((float) n/2))
                    {
                        if (x > y)
                        {
                            winners[x, y] = x;
                        }
                        else
                        {
                            winners[x, y] = y;
                        }
                    }
                    else
                    {
                        if (x < y)
                        {
                            winners[x, y] = x;
                        }
                        else
                        {
                            winners[x, y] = y;
                        }
                    }
                }
            }
        }

        private static int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}

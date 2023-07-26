using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cellophain
{
    class GeneSheep : Element
    {
        int id;
        int n;
        int mutationStrength;

        Random rand = new Random();
        public GeneSheep(int red, int green, int blue, int id, int n)
        {
            vars["name"] = "geneSheep";
            vars["r"] = red;
            vars["g"] = green;
            vars["b"] = blue;
            this.id = id;
            this.n = n;
        }
        public override Request Iterate(Element[,] world)
        {
            List<Instruction> instructions = new List<Instruction>();
            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;
            int winningId = GetWinningId(world, xPos, yPos);
            Color winningColor = GetWinningColor(world, xPos, yPos, winningId);
            GeneSheep newSheep;
            mutationStrength = 10;

            if (winningId == id)
            {
                newSheep = this;
            }
            else
            {
                newSheep = new GeneSheep(winningColor.R + rand.Next(-mutationStrength, mutationStrength+1), winningColor.G + rand.Next(-mutationStrength, mutationStrength+1), winningColor.B + rand.Next(-mutationStrength, mutationStrength+1), winningId, n);
            }

            instructions.Add(new Instruction(xPos, yPos, newSheep));
            return new Request(instructions);
        }

        private int GetWinningId(Element[,] world, int xPos, int yPos)
        {
            int[] neighbors = new int[n];

            for (int x = xPos - 1; x <= xPos + 1; x++)
            {
                for (int y = yPos - 1; y <= yPos + 1; y++)
                {
                   if (CheckCell(world, x, y).GetName() == "geneSheep")
                   {
                        GeneSheep toCheck = (GeneSheep) CheckCell(world, x, y);
                        if (!(y == yPos && x == xPos))
                        {
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

            return winningId;
        }

        private Color GetWinningColor(Element[,] world, int xPos, int yPos, int winningId)
        {
            List<GeneSheep> winners = new List<GeneSheep>();

            for (int x = xPos - 1; x <= xPos + 1; x++)
            {
                for (int y = yPos - 1; y <= yPos + 1; y++)
                {
                    if (CheckCell(world, x, y).GetName() == "geneSheep")
                    {
                        GeneSheep toCheck = (GeneSheep) CheckCell(world, x, y);

                        if (!(y == yPos && x == xPos) && toCheck.id == winningId)
                        {
                            winners.Add(toCheck);
                        }
                    }
                }
            }

            if (winners.Count > 0)
            {
                return winners[0].GetColor();
            }
            else
            {
                return this.GetColor();
            }

        }

        private int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
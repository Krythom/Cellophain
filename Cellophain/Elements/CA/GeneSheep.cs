using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cellophain
{
    class GeneSheep : Element
    {
        int n;
        int mutationStrength;
        List<GeneSheep> neighbors = new();

        Random rand = new Random();
        public GeneSheep(int red, int green, int blue, int id, int n)
        {
            vars["name"] = "geneSheep";
            vars["r"] = red;
            vars["g"] = green;
            vars["b"] = blue;
            vars["id"] = id;
            vars["sleep"] = false;
            this.n = n;
        }
        public override Request Iterate(Element[,] world)
        {
            if (neighbors.Count == 0)
            {
                this.neighbors = GetNeighbors(world, GetLocation().X, GetLocation().Y);
            }

            if (!IsSleeping())
            {
                List<Instruction> instructions = new List<Instruction>();
                int xPos = GetLocation().X;
                int yPos = GetLocation().Y;
                int winningId = GetWinningId(world, xPos, yPos);
                Color winningColor = GetWinningColor(world, xPos, yPos, winningId);
                mutationStrength = 10;

                if (winningId != this.GetID())
                {
                    instructions.Add(new Instruction(this, "r", winningColor.R + rand.Next(-mutationStrength, mutationStrength + 1)));
                    instructions.Add(new Instruction(this, "g", winningColor.G + rand.Next(-mutationStrength, mutationStrength + 1)));
                    instructions.Add(new Instruction(this, "b", winningColor.B + rand.Next(-mutationStrength, mutationStrength + 1)));
                    instructions.Add(new Instruction(this, "id", winningId));
                    foreach (GeneSheep g in neighbors)
                    {
                        instructions.Add(new Instruction(g, "sleep", false));
                    }
                }

                return new Request(instructions);
            }
            else
            {
                return null;
            }
        }

        private int GetWinningId(Element[,] world, int xPos, int yPos)
        {
            int[] neighborIDs = new int[n];

            foreach (GeneSheep g in neighbors)
            {
                neighborIDs[g.GetID()]++;
            }

            int winningId = -1;
            int highestCount = -1;

            for (int i = 0; i < neighborIDs.Length; i++)
            {
                if (neighborIDs[i] > highestCount)
                {
                    winningId = i;
                    highestCount = neighborIDs[i];
                }
                else if (neighborIDs[i] == highestCount && rand.NextDouble() < 0.5)
                {
                    winningId = i;
                    highestCount = neighborIDs[i];
                }
            }

            if (neighborIDs[GetID()] == neighbors.Count)
            {
                vars["sleep"] = true; 
            }

            return winningId;
        }

        private Color GetWinningColor(Element[,] world, int xPos, int yPos, int winningId)
        {
            List<GeneSheep> winners = new List<GeneSheep>();

            foreach (GeneSheep g in neighbors)
            {
                if (g.GetID() == winningId)
                {
                    winners.Add(g);
                }
            }

            return winners[winners.Count/2].GetColor();
        }

        private List<GeneSheep> GetNeighbors(Element[,] world, int xPos, int yPos)
        {
            List<GeneSheep> neighbors = new();

            for (int x = xPos - 1; x <= xPos + 1; x++)
            {
                for (int y = yPos - 1; y <= yPos + 1; y++)
                {
                    var cell = CheckCell(world, x, y);
                    if (cell is GeneSheep toAdd)
                    {
                        if (!(y == yPos && x == xPos))
                        {
                            neighbors.Add(toAdd);
                        }
                    }
                }
            }

            return neighbors;
        }

        private int GetID()
        {
            return System.Convert.ToInt32(vars["id"]);
        }

        private bool IsSleeping()
        {
            return System.Convert.ToBoolean(vars["sleep"]);
        }
    }
}
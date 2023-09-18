using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    class Bobbit : Element
    {
        Random rand = new Random();
        public Bobbit(int red, int green, int blue, int id, double strength)
        {
            vars["strength"] = strength;
            vars["id"] = id;
            vars["r"] = red;
            vars["g"] = green;
            vars["b"] = blue;
        }

        public override Request Iterate(Element[,] world)
        {
            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;

            List<Instruction> instructions = new List<Instruction>();
            Bobbit winner = GetStrongest(world, xPos, yPos);
            Color win = winner.GetColor();
            if (winner.GetId() != GetId())
            {
                instructions.Add(new Instruction(xPos, yPos, new Bobbit(win.R, win.G, win.B, winner.GetId(), winner.GetStrength() + rand.NextDouble() - 0.75)));
            }
            else
            {
                instructions.Add(new Instruction(this, "strength", GetStrength() + (rand.NextDouble() - 1)/10));
            }
            return new Request(instructions);
        }

        public Bobbit GetStrongest(Element[,] world, int xPos, int yPos)
        {
            double strongest = 0;
            Point strongestLoc = new Point(xPos,yPos);

            for (int x = -1; x <= 1; x++)
            {
                for (int y =  -1; y <= 1; y++)
                {
                    if (!(x == 0 && y == 0))
                    {
                        var cell = CheckCell(world, xPos + x, yPos + y);
                        if (cell is Bobbit toCheck)
                        {
                            if (toCheck.GetStrength() >= strongest)
                            {
                                strongest = toCheck.GetStrength();
                                strongestLoc = toCheck.GetLocation();
                            }
                        }
                    }
                }
            }

            Element winner = world[strongestLoc.X, strongestLoc.Y];
            return (Bobbit)winner;

        }

        public List<Point> GetVoids(Element[,] world, int xPos, int yPos)
        {
            List<Point> voids = new List<Point>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var cell = CheckCell(world, xPos + x, yPos + y);
                    if (cell is Void toCheck)
                    {
                        voids.Add(toCheck.GetLocation());
                    }
                }
            }
            return voids;
        }

        public double GetStrength()
        {
            return System.Convert.ToDouble(vars["strength"]);
        }

        public int GetId()
        {
            return System.Convert.ToInt32(vars["id"]);
        }
    }
}

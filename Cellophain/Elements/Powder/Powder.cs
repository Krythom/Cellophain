using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    abstract class Powder : Element
    {
        Random rand = new Random();
        public string GetMatter()
        {
            return System.Convert.ToString(vars["matterType"]);
        }

        public double GetDensity()
        {
            return System.Convert.ToDouble(vars["density"]);
        }

        public double GetTemp() 
        {
            return System.Convert.ToDouble(vars["temp"]);
        }

        public double GetCapacity()
        {
            return System.Convert.ToDouble(vars["heatCapacity"]);
        }

        public double TempChange(Element[,] world, Powder self)
        {
            int xPos = self.GetLocation().X;
            int yPos = self.GetLocation().Y;
            double env = 0;
            int cells = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (Math.Abs(x) != Math.Abs(y))
                    {
                        Powder other = (Powder) CheckCell(world, x + xPos, y + yPos);
                        if (other.GetName() != "boundary")
                        {
                            env += other.GetTemp();
                            cells++;
                        }
                    }
                }
            }

            env /= cells;
            double change = 0;

            if (self.GetTemp() > env)
            {
                change = Math.Max((env - self.GetTemp()) / (self.GetCapacity() * self.GetDensity()), env - self.GetTemp());
            }
            else if (self.GetTemp() < env)
            {
                change = Math.Min((env - self.GetTemp()) / (self.GetCapacity() * self.GetDensity()), env - self.GetTemp());
            }

            return change;

        }

        public List<Instruction> PowderUpdate(Element[,] world, Powder self, List<Instruction> instructions)
        {
            int xPos = self.GetLocation().X;
            int yPos = self.GetLocation().Y;
            Powder down = (Powder)CheckCell(world, xPos, yPos + 1);
            Powder downLeft = (Powder)CheckCell(world, xPos - 1, yPos + 1);
            Powder downRight = (Powder)CheckCell(world, xPos + 1, yPos + 1);

            if (down.GetMatter() is "gas" or "liquid")
            {
                if (rand.NextDouble() > down.GetDensity() / self.GetDensity())
                {
                    instructions.Add(new Instruction(xPos, yPos, down));
                    instructions.Add(new Instruction(xPos, yPos + 1, self));
                }
            }
            else
            {
                //Randomize order of directional checks to avoid left bias
                Powder[] sides = { downLeft, downRight };
                int first = rand.Next(2);

                if (sides[first].GetMatter() is "gas" or "liquid")
                {
                    if (rand.NextDouble() > sides[first].GetDensity() / self.GetDensity())
                    {
                        instructions.Add(new Instruction(xPos, yPos, sides[first]));
                        instructions.Add(new Instruction(xPos + (2 * first) - 1, yPos + 1, self));
                    }
                }
                else if (sides[(first + 1) % 2].GetMatter() is "gas" or "liquid")
                {
                    if (rand.NextDouble() > sides[first].GetDensity() / self.GetDensity())
                    {
                        instructions.Add(new Instruction(xPos, yPos, sides[(first + 1) % 2]));
                        instructions.Add(new Instruction(xPos + (2 * ((first + 1) % 2)) - 1, yPos + 1, self));
                    }
                }
            }

            return instructions;
        }

        public List<Instruction> FluidUpdate(Element[,] world, Powder self, List<Instruction> instructions)
        {
            int xPos = GetLocation().X;
            int yPos = GetLocation().Y;
            Powder left = (Powder)CheckCell(world, xPos - 1, yPos);
            Powder right = (Powder)CheckCell(world, xPos + 1, yPos);
            Powder down = (Powder)CheckCell(world, xPos, yPos + 1);

            if (down.GetMatter() is "gas" or "liquid" && down.GetName() != self.GetName())
            {
                if (rand.NextDouble() > down.GetDensity() / self.GetDensity())
                {
                    instructions.Add(new Instruction(xPos, yPos, down));
                    instructions.Add(new Instruction(xPos, yPos + 1, self));
                }
            }
            else
            {
                //Randomize order of directional checks to avoid left bias
                Powder[] sides = { left, right };
                int first = rand.Next(2);

                if (sides[first].GetMatter() is "gas" or "liquid" && sides[first].GetName() != self.GetName())
                {
                    if ((rand.NextDouble() > sides[first].GetDensity() / self.GetDensity()))
                    {
                        instructions.Add(new Instruction(xPos, yPos, sides[first]));
                        instructions.Add(new Instruction(xPos + (2 * first) - 1, yPos, self));
                    }
                }
                else if (sides[(first + 1) % 2].GetMatter() is "gas" or "liquid" && sides[(first + 1) % 2].GetName() != self.GetName())
                {
                    if (rand.NextDouble() > sides[(first + 1) % 2].GetDensity() / self.GetDensity())
                    {
                        instructions.Add(new Instruction(xPos, yPos, sides[(first + 1) % 2]));
                        instructions.Add(new Instruction(xPos + (2 * ((first + 1) % 2)) - 1, yPos, self));
                    }
                }
            }

            return instructions;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    abstract class Powder : Element
    {
        public string GetMatter()
        {
            return (string) vars["matterType"];
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

        public double TempChange(Element[,] world, Powder self, int xPos, int yPos)
        {
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
    }
}
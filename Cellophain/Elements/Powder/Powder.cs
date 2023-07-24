using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellophain
{
    abstract class Powder : Element
    {
        protected string matterType;
        protected double density;
        protected double temp;

        public string GetMatter()
        {
            return matterType;
        }

        public double GetDensity()
        {
            return density;
        }

        public double GetTemp() 
        {
            return temp;
        }

        //Avoids needing to cast as a powder for every check in individual powders code
        public Powder CheckPowder(Element[,] world, int x, int y)
        {
            return (Powder) CheckCell(world, x, y);
        }
    }
}
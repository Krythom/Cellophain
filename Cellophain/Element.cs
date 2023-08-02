using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cellophain
{
    abstract class Element
    {
        protected Hashtable vars = new Hashtable
        {
            { "name", "" },
            { "r", 0 },
            { "g", 0 },
            { "b", 0 },
            { "x", 0 },
            { "y", 0 }
        };

        public abstract Request Iterate(Element[,] world);

        //Override if an Element needs to do something on creation (such as randomize a property)
        public virtual void Initialize()
        {
        }

        public string GetName()
        {
            return System.Convert.ToString(vars["name"]);
        }

        public Color GetColor()
        {
            int r = System.Convert.ToInt32(vars["r"]);
            int g = System.Convert.ToInt32(vars["g"]);
            int b = System.Convert.ToInt32(vars["b"]);
            return new Color(r, g, b);
        }

        public Hashtable GetVars()
        {
            return vars;
        }

        public Point GetLocation()
        {
            int x = System.Convert.ToInt32(vars["x"]);
            int y = System.Convert.ToInt32(vars["y"]);
            return new Point(x, y);
        }

        public void SetLocation(Point location)
        {
            vars["x"] = location.X;
            vars["y"] = location.Y;
        }

        public Element CheckCell(Element[,] world, int x, int y)
        {
            if (x >= 0 && y >= 0 && x < world.GetLength(0) && y < world.GetLength(0))
            {
                return world[x, y];
            }
            else
            {
                return new Boundary();
            }
        }

        public object DeepCopy()
        {
            Element copy = (Element) this.MemberwiseClone();
            copy.vars = new Hashtable();
            foreach (DictionaryEntry entry in vars)
            {
                copy.vars[entry.Key] = entry.Value;
            }
            return copy;
        }
    }
}

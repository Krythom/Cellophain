using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Linq.Expressions;

namespace Cellophain
{
    class Iterator
    {
        private int iterationCount;
        private int gridSize;
        private int priorityType;
        List<Point> indices;
        Random rand = new Random();

        public Iterator(int gridSize, int priorityType)
        {
            this.gridSize = gridSize;
            this.priorityType = priorityType;

            if(priorityType == 1)
            {
                indices = new List<Point>();
                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        indices.Add(new Point(x, y));
                    }
                }
            }
        }

        public Element[,] Iterate(Element[,] world)
        {
            Element[,] newWorld = new Element[gridSize, gridSize];

            //Copy current world data into the temporary new world
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    newWorld[x, y] = world[x, y];
                }
            }

            if (priorityType == 0)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        Request request = world[x, y].Iterate(world);
                        if (request != null)
                        {
                            foreach (Instruction instruction in request.GetInstructions())
                            {
                                if (instruction.InstructionType() == "move")
                                {
                                    newWorld[instruction.GetCoords().X, instruction.GetCoords().Y] = instruction.GetElement();
                                    Hashtable vars = instruction.GetElement().GetVars();
                                    vars["x"] = instruction.GetCoords().X;
                                    vars["y"] = instruction.GetCoords().Y;
                                }
                                else
                                {
                                    Point loc = instruction.GetElement().GetLocation();
                                    Hashtable vars = newWorld[loc.X, loc.Y].GetVars();
                                    vars[instruction.GetKey()] = instruction.GetVal();
                                }
                            }
                        }
                    }
                }
            }
            else if (priorityType == 1)
            {
                Shuffle<Point>(indices);
                foreach (Point p in indices)
                {
                    Request request = world[p.X, p.Y].Iterate(newWorld);
                    if (request != null)
                    {
                        foreach (Instruction instruction in request.GetInstructions())
                        {
                            if (instruction.InstructionType() == "move")
                            {
                                newWorld[instruction.GetCoords().X, instruction.GetCoords().Y] = instruction.GetElement();
                                Hashtable vars = instruction.GetElement().GetVars();
                                vars["x"] = instruction.GetCoords().X;
                                vars["y"] = instruction.GetCoords().Y;
                            }
                            else
                            {
                                Point loc = instruction.GetElement().GetLocation();
                                Hashtable vars = newWorld[loc.X, loc.Y].GetVars();
                                vars[instruction.GetKey()] = instruction.GetVal();
                            }
                        }
                    }
                }
            }
            else if (priorityType == 2)
            {
                //Generate list of all requests for cell changes
                List<Request> requests = new List<Request>();

                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        Request request = world[x, y].Iterate(world);
                        if (request != null)
                        {
                            requests.Add(world[x, y].Iterate(world));
                        }
                    }
                }

                newWorld = ExecuteRequests(requests, newWorld);
            }

            iterationCount++;
            return newWorld;
        }

        //Execute requests while dealing with conflicts
        //TODO: Doesn't currently handle "value" requests
        private Element[,] ExecuteRequests(List<Request> requests, Element[,] newWorld)
        {
            List<Request> filteredRequests = requests;
            filteredRequests = filteredRequests.OrderByDescending(o => o.GetPriority()).ToList();
            bool[,] affectedCells = new bool[gridSize, gridSize];

            while(filteredRequests.Count > 0)
            {
                float currentPriority = filteredRequests[0].GetPriority();
                List<Request> priorityGroup = new List<Request>();

                int n = 0;
                while (n < filteredRequests.Count && filteredRequests[n].GetPriority() == currentPriority)
                {
                    priorityGroup.Add(filteredRequests[n]);
                    n++;
                }

                Shuffle<Request>(priorityGroup);

                foreach (Request r in priorityGroup)
                {
                    bool validRequest = true;

                    foreach(Instruction instruction in r.GetInstructions())
                    {
                        if (affectedCells[instruction.GetCoords().X, instruction.GetCoords().Y] == true)
                        {
                            validRequest = false;
                            break;
                        }
                    }

                    if (validRequest)
                    {
                        foreach (Instruction instruction in r.GetInstructions())
                        {
                            affectedCells[instruction.GetCoords().X, instruction.GetCoords().Y] = true;
                            newWorld[instruction.GetCoords().X, instruction.GetCoords().Y] = instruction.GetElement();
                        }
                    }

                    filteredRequests.Remove(r);
                }
            }

            return newWorld;
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                var k = rand.Next(i + 1);
                var value = list[k];
                list[k] = list[i];
                list[i] = value;
            }
        }

        public int GetIterations()
        {
            return iterationCount;
        }
    }
}
